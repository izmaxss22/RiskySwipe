using UnityEngine;

//todo
public class SubsScreen_Purchases : MonoBehaviour
{
    //public DataManager_AllData DataManager_AllData;
    //public Panel_EnergyAndCoinsDesc Panel_EnergyAndCoinsDesc;

    //public GameObject contFor_energyAndAd;
    //public GameObject contFor_endlessEnergy;
    //public GameObject contFor_adRemoving;

    //private void Start()
    //{
    //    bool adIsRemoved = DataManager_AllData.Get_AdIsRemovedState();
    //    bool energyIsEndles = DataManager_AllData.Get_EnergyIsEndlessState();
    //    if (adIsRemoved) contFor_adRemoving.SetActive(false);
    //    if (energyIsEndles) contFor_endlessEnergy.SetActive(false);
    //    if (adIsRemoved && energyIsEndles) contFor_energyAndAd.SetActive(false);
    //}

    //public void OnClick_BuyEndlessEnergyButton()
    //{
    //    MMVibrationManager.Haptic(HapticTypes.SoftImpact);

    //    IAPManager.instance.PurchaseItem(
    //        EM_IAPConstants.Product_shop_screen_endless_energy_2,
    //        (bool purchaseIsComplete) =>
    //        {
    //            // Если покупка была успешна то по завершению отключаю окно с для покупки энергии
    //            if (purchaseIsComplete)
    //            {
    //                DataManager_AllData.Set_EnergyCount(25);
    //                EnergyAddingManager.instance.Disable_EnergAddingTimer();
    //                Panel_EnergyAndCoinsDesc.Instance.OnChangeEnergyCount();

    //                contFor_endlessEnergy.SetActive(false);
    //                if (DataManager_AllData.Get_AdIsRemovedState())
    //                    contFor_energyAndAd.SetActive(false);
    //            }
    //        });
    //}
    //public void OnClick_BuyRemoveAdButton()
    //{
    //    MMVibrationManager.Haptic(HapticTypes.SoftImpact);

    //    IAPManager.instance.PurchaseItem(
    //      EM_IAPConstants.Product_gravity_jump_screen_shop_remove_ad,
    //      (bool purchaseIsComplete) =>
    //      {
    //          // Если покупка была успешна то по завершению отключаю окно с для покупки удаления рекламы
    //          if (purchaseIsComplete)
    //          {
    //              contFor_adRemoving.SetActive(false);
    //              if (DataManager_AllData.Get_EnergyIsEndlessState())
    //                  contFor_energyAndAd.SetActive(false);
    //          }
    //      });
    //}
    //public void OnClick_BuyKit1Button()
    //{
    //    MMVibrationManager.Haptic(HapticTypes.SoftImpact);

    //    IAPManager.instance.PurchaseItem(EM_IAPConstants.Product_gravity_jump_screen_shop_kit_1, (bool purchaseIsComplete) =>
    //    {
    //        if (purchaseIsComplete)
    //        {
    //            MMVibrationManager.Haptic(HapticTypes.Failure);

    //            Panel_EnergyAndCoinsDesc.Instance.OnChange_CoinsCount();
    //        }
    //    });
    //}
    //public void OnClick_BuyKit2Button()
    //{
    //    MMVibrationManager.Haptic(HapticTypes.SoftImpact);

    //    IAPManager.instance.PurchaseItem(EM_IAPConstants.Product_gravity_jump_screen_shop_kit_2, (bool purchaseIsComplete) =>
    //    {
    //        if (purchaseIsComplete)
    //        {
    //            MMVibrationManager.Haptic(HapticTypes.Failure);

    //            Panel_EnergyAndCoinsDesc.Instance.OnChange_CoinsCount();
    //        }
    //    });
    //}
    //public void OnClick_BuyKit3Button()
    //{
    //    MMVibrationManager.Haptic(HapticTypes.SoftImpact);

    //    IAPManager.instance.PurchaseItem(EM_IAPConstants.Product_gravity_jump_screen_shop_coins, (bool purchaseIsComplete) =>
    //    {
    //        if (purchaseIsComplete)
    //        {
    //            MMVibrationManager.Haptic(HapticTypes.Failure);

    //            Panel_EnergyAndCoinsDesc.Instance.OnChange_CoinsCount();
    //        }
    //    });
    //}
}
