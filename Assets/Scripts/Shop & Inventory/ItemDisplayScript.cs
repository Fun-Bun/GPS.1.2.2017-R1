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

	public Animator anim;
	
	// Update is called once per frame
	void Start ()
	{
		itemIcon.sprite = Item.GetSprite(type);
		nameText.text = Item.GetName (type);
		priceText.text = Item.GetPrice (type).ToString();
		descText.text = Item.GetDesc (type);
	}

	void OnEnable()
	{
		if(anim == null) anim = GetComponent<Animator>();

		switch(type)
		{
			case Item.Type.A:
				anim.Play("Vaccine_A");
				break;
			case Item.Type.B:
				anim.Play("MPA");
				break;
			case Item.Type.C:
				anim.Play("Bulletform");
				break;
		}
	}
}
