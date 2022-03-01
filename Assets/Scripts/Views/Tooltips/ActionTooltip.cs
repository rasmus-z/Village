using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Village.Controllers;
using Village.Scriptables;
using static Village.Scriptables.Effect;
using static Village.Scriptables.Resource;

namespace Village.Views.Tooltips
{
	[SelectionBase]
	public class ActionTooltip : Tooltip<ActionSlot>
	{
		[SerializeField]
		private TMP_Text actionName;

		[SerializeField]
		private TMP_Text actionDescription;

		[SerializeField]
		private Image statIcon1, statIcon2;

		[SerializeField]
		private Sprite defaultIcon;

		[SerializeField]
		private Transform effectsParent;

		[SerializeField]
		private EffectView actionResultPrefab;

		[SerializeField]
		private Color defaultValue;

		[SerializeField]
		private Color effectiveValue;

		[SerializeField]
		private Color missingValue;

		public override void Load(ActionSlot slot)
		{
			if (slot.Action == null)
			{
				Debug.LogWarning("Action is not set!");
				return;
			}

			Clear();
			actionName.text = slot.Action.ActionName;

			bool hasDescription = slot.Action.Description != null;
			bool hideDescription = GameSettings.SimplifiedTooltips;
			if (hasDescription)
			{
				actionDescription.text = slot.Action.Description;
			}
			actionDescription.transform.parent.gameObject.SetActive(hasDescription && !hideDescription);

			if (slot.Action.Stat1)
			{
				statIcon1.sprite = slot.Action.Stat1.backgroundIcon;
				statIcon1.color = slot.Action.Stat1.color;
			}
			else
			{
				statIcon1.sprite = defaultIcon;
				statIcon1.color = defaultValue;
			}

			if (slot.Action.Stat2)
			{
				statIcon2.gameObject.SetActive(true);
				statIcon2.sprite = slot.Action.Stat2.backgroundIcon;
				statIcon2.color = slot.Action.Stat2.color;
			}
			else statIcon2.gameObject.SetActive(false);

			foreach (var cost in slot.Action.Costs)
			{
				LoadCost(cost);
			}

			foreach (var eff in slot.Action.Effects)
			{
				float multiplier = slot.Action.GetMultiplier(slot.Villager);
				LoadEffect(eff, multiplier);
			}

			RefreshLayout();
		}

		private void Clear()
		{
			foreach (Transform item in effectsParent)
			{
				Destroy(item.gameObject);
			}
		}

		private void LoadEffect(EffectAmount eff, float multiplier)
		{
			var actionResult = Instantiate(actionResultPrefab, effectsParent);
			actionResult.SetIcon(eff.effect.icon);
			actionResult.SetIconColor(eff.effect.color);
			int effectiveAmount = Mathf.RoundToInt(eff.value * multiplier);
			actionResult.SetAmount(effectiveAmount);
			if (effectiveAmount > eff.value)
			{
				actionResult.SetFontColor(effectiveValue);
				//actionResult.SetBold(true);
			}
			else
			{
				actionResult.SetFontColor(defaultValue);
				//actionResult.SetBold(false);
			}
		}

		private void LoadCost(ResourceAmount res)
		{
			var actionResult = Instantiate(actionResultPrefab, effectsParent);
			actionResult.SetIcon(res.resource.icon);
			actionResult.SetIconColor(res.resource.color);
			actionResult.SetAmount(-res.Amount);
			if(GameController.instance.GetResourceAmount(res.resource) < res.Amount)
			{
				actionResult.SetFontColor(missingValue);
				//actionResult.SetBold(true);
			}
			else
			{
				actionResult.SetFontColor(defaultValue);
				//actionResult.SetBold(false);
			}
		}



		private void RefreshLayout()
		{
			GetComponent<LayoutRefresher>().RefreshContentFitters();
			GetComponent<LayoutRefresher>().RefreshContentFitters();
		}
	}
}
