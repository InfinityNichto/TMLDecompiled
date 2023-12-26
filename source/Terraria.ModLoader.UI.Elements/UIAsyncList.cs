using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.UI.Elements;

/// <remarks>
///   Remember to set GenElement is not provided in the constructor and TResource is not a TUIElement.
///   DO NOT USE Add/AddRange directly, always use the provider methods.
/// </remarks>
public abstract class UIAsyncList<TResource, TUIElement> : UIList where TUIElement : UIElement
{
	public delegate void StateDelegate(AsyncProviderState state);

	public delegate void StateDelegateWithException(AsyncProviderState state, Exception e);

	private bool ProviderChanged;

	private AsyncProvider<TResource> Provider;

	private UIText EndItem;

	public AsyncProviderState State { get; private set; } = AsyncProviderState.Completed;


	private AsyncProviderState RealtimeState => Provider?.State ?? AsyncProviderState.Completed;

	public IEnumerable<TUIElement> ReceivedItems
	{
		get
		{
			using IEnumerator<UIElement> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				UIElement el = enumerator.Current;
				if (el != EndItem)
				{
					yield return el as TUIElement;
				}
			}
		}
	}

	public event StateDelegate OnStartLoading;

	public event StateDelegateWithException OnFinished;

	protected abstract TUIElement GenElement(TResource resource);

	public UIAsyncList()
	{
		ManualSortMethod = delegate
		{
		};
	}

	/// <remarks>
	///   SetProvider will delegate all UI actions to next Update,
	///   so it NOT SAFE to be called out of the main thread,
	///   because having an assignment to ProviderChanged it CAN
	///   cause problems in case the list is cleared before the provider
	///   is swapped and the old provider is partially read giving unwanted
	///   elements, same if you do the other way around (the provider can be
	///   partially consumed before the clear)
	/// </remarks>
	public void SetProvider(AsyncProvider<TResource> provider = null)
	{
		Provider?.Cancel();
		ProviderChanged = true;
		Provider = provider;
	}

	public void SetEnumerable(IAsyncEnumerable<TResource> enumerable = null)
	{
		if (enumerable != null)
		{
			SetProvider(new AsyncProvider<TResource>(enumerable));
		}
		else
		{
			SetProvider();
		}
	}

	public override void Update(GameTime gameTime)
	{
		bool endItemTextNeedUpdate = false;
		if (ProviderChanged)
		{
			Clear();
			Add(EndItem);
			ProviderChanged = false;
			InternalOnUpdateState(AsyncProviderState.Loading);
			endItemTextNeedUpdate = true;
		}
		if (Provider != null)
		{
			TUIElement[] uiels = Provider.GetData().Select(GenElement).ToArray();
			if (uiels.Length != 0)
			{
				Remove(EndItem);
				AddRange(uiels);
				Add(EndItem);
			}
		}
		AsyncProviderState providerState = RealtimeState;
		if (providerState != State)
		{
			InternalOnUpdateState(providerState);
			endItemTextNeedUpdate = true;
		}
		if (endItemTextNeedUpdate)
		{
			EndItem.SetText(GetEndItemText());
		}
		base.Update(gameTime);
	}

	private void InternalOnUpdateState(AsyncProviderState state)
	{
		State = state;
		if (State.IsFinished())
		{
			this.OnFinished(State, Provider?.Exception);
		}
		else
		{
			this.OnStartLoading(State);
		}
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
		EndItem = new UIText(GetEndItemText())
		{
			HAlign = 0.5f
		}.WithPadding(15f);
		Add(EndItem);
	}

	public virtual string GetEndItemText()
	{
		switch (State)
		{
		case AsyncProviderState.Loading:
			return Language.GetTextValue("tModLoader.ALLoading");
		case AsyncProviderState.Completed:
			if (!ReceivedItems.Any())
			{
				return Language.GetTextValue("tModLoader.ALNoEntries");
			}
			return "";
		case AsyncProviderState.Canceled:
			return Language.GetTextValue("tModLoader.ALCancel");
		case AsyncProviderState.Aborted:
			return Language.GetTextValue("tModLoader.ALError");
		default:
			return "Invalid state";
		}
	}

	public void Cancel()
	{
		Provider?.Cancel();
	}
}
