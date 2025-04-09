using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] GameObject ArrowPrefab;
	[SerializeField] SpriteRenderer ArrowGFX;
	[SerializeField] Slider BowPowerSlider;
	[SerializeField] Transform Bow;

	[Range(0, 20)]
	[SerializeField] float BowPower;

	[Range(0, 3)]
	[SerializeField] float MaxBowCharge;
	
	float BowCharge;
	bool CanFire = true;

	private void Start() {
		BowPowerSlider.value = 0f;
		BowPowerSlider.maxValue = MaxBowCharge;
	}

	private void Update() {
		if (Input.GetMouseButton(0) && CanFire) {
			ChargeBow();
		} else if (Input.GetMouseButtonUp(0) && CanFire) {
			FireBow();
		} else {
			if (BowCharge > 0f) {
				BowCharge -= 1f * Time.deltaTime;
			} else {
				BowCharge = 0f;
				CanFire = true;
			}

			BowPowerSlider.value = BowCharge;
		}
	}

	void ChargeBow() {
		ArrowGFX.enabled = true;
		
		BowCharge += Time.deltaTime;

		BowPowerSlider.value = BowCharge;

		if (BowCharge > MaxBowCharge) {
			BowPowerSlider.value = MaxBowCharge;
		}
	}

    void FireBow()
    {
        if (BowCharge > MaxBowCharge) BowCharge = MaxBowCharge;

        float ArrowSpeed = BowCharge + BowPower;
        float ArrowDamage = BowCharge * BowPower;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - Bow.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        Arrow arrow = Instantiate(ArrowPrefab, Bow.position, rot).GetComponent<Arrow>();
        arrow.arrowSpeed = ArrowSpeed;
        arrow.ArrowDamage = ArrowDamage;
        arrow.arrowDirection = direction; // <- gán hướng bay đúng

        CanFire = false;
        ArrowGFX.enabled = false;
    }
}