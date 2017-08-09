using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ItemDisplayScript : MonoBehaviour
{
	public Item.Type type;

	public Image itemIcon;
	public Text nameText;
	public Text priceText;
	public Text descText;
	
	// Update is called once per frame
	void Start ()
	{
		itemIcon.sprite = Item.GetSprite(type);
		nameText.text = Item.GetName (type);
		priceText.text = Item.GetPrice (type).ToString();
		descText.text = Item.GetDesc (type);
	}
}
