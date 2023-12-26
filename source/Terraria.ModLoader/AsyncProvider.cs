using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Terraria.ModLoader;

public class AsyncProvider<T>
{
	private Channel<T> _Channel;

	private CancellationTokenSource TokenSource;

	public bool IsCancellationRequested => TokenSource.IsCancellationRequested;

	public AsyncProviderState State
	{
		get
		{
			Task completion = _Channel.Reader.Completion;
			if (!completion.IsCompleted)
			{
				return AsyncProviderState.Loading;
			}
			if (completion.IsCanceled)
			{
				return AsyncProviderState.Canceled;
			}
			if (completion.IsFaulted)
			{
				return AsyncProviderState.Aborted;
			}
			return AsyncProviderState.Completed;
		}
	}

	public Exception Exception => _Channel.Reader.Completion.Exception;

	/// <remarks>
	///   Remember to provide your enumerator with
	///   `[EnumeratorCancellation] CancellationToken token = default`
	///   as argument to allow cancellation notification.
	/// </remarks>
	public AsyncProvider(IAsyncEnumerable<T> provider)
	{
		AsyncProvider<T> asyncProvider = this;
		_Channel = Channel.CreateUnbounded<T>();
		TokenSource = new CancellationTokenSource();
		((Func<Task>)async delegate
		{
			ChannelWriter<T> writer = asyncProvider._Channel.Writer;
			try
			{
				await foreach (T item in provider.WithCancellation(asyncProvider.TokenSource.Token))
				{
					await writer.WriteAsync(item);
				}
				writer.Complete();
			}
			catch (Exception ex)
			{
				writer.Complete(ex);
			}
		})();
	}

	public void Cancel()
	{
		TokenSource.Cancel();
	}

	public IEnumerable<T> GetData()
	{
		T item;
		while (_Channel.Reader.TryRead(out item))
		{
			yield return item;
		}
	}
}
