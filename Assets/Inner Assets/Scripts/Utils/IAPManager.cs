using System;
//using EasyMobile;
using UnityEngine;

//todo
public class IAPManager : MonoBehaviour
{
    //public DataManager_AllData DataManager_AllData;
    //private event Action<bool> PurchaseEvent;

    //private IAPProduct[] products;
    //private bool isInitialized;

    //public static IAPManager instance;

    //private void Awake()
    //{
    //    instance = this;
    //    isInitialized = InAppPurchasing.IsInitialized();
    //    if (isInitialized == false) InAppPurchasing.InitializePurchasing();
    //    products = InAppPurchasing.GetAllIAPProducts();
    //}

    //// todo при бесконечной энергии откключить эту покупку
    //// разобраться с ask to buy
    //// проверить поведение востановления покупок на айндрод
    //// добавить кнопку вост покупок в настройки
    //// сделать бонусный покуки (энергия со скидкой немного возрождений)
    //// протестировать праввильно ли работают все покупки


    //private void OnEnable()
    //{
    //    InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
    //    InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;

    //    InAppPurchasing.RestoreCompleted += RestoreCompletedHandler;
    //    InAppPurchasing.RestoreFailed += RestoreFailedHandler;
    //}

    //private void OnDisable()
    //{
    //    InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
    //    InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

    //    InAppPurchasing.RestoreCompleted -= RestoreCompletedHandler;
    //    InAppPurchasing.RestoreFailed -= RestoreFailedHandler;
    //}


    //public void PurchaseItem(string purchaseName, Action<bool> callbackMethod)
    //{
    //    if (callbackMethod != null)
    //        PurchaseEvent += callbackMethod;
    //    switch (purchaseName)
    //    {
    //        case EM_IAPConstants.Product_shop_screen_endless_energy_2:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_shop_screen_endless_energy_2);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_remove_ad:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_gravity_jump_screen_shop_remove_ad);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_coins:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_gravity_jump_screen_shop_coins);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_kit_1:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_gravity_jump_screen_shop_kit_1);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_kit_2:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_gravity_jump_screen_shop_kit_2);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_player_skin_1_0:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_gravity_jump_screen_shop_player_skin_1_0);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_player_skin_2:
    //            InAppPurchasing.Purchase(EM_IAPConstants.Product_gravity_jump_screen_shop_player_skin_2);
    //            break;
    //        default:
    //            break;

    //    }
    //}

    //private void PurchaseCompletedHandler(IAPProduct product)
    //{
    //    // Compare product name to the generated name constants to determine which product was bought
    //    switch (product.Name)
    //    {
    //        case EM_IAPConstants.Product_shop_screen_endless_energy_2:
    //            DataManager_AllData.Set_EnergyIsEndlessState(true);
    //            //todo сделать смену иконки на бесконечный и разобраться с мененджером энергии и отниманием при начале игры
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_remove_ad:
    //            DataManager_AllData.Set_AdIsRemovedState(true);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_kit_1:
    //            DataManager_AllData.Set_CoinsCount(DataManager_AllData.Get_CoinsCount() + 5550);
    //            DataManager_AllData.Set_ShieldEffectsCount(DataManager_AllData.Get_ShieldEffectsCount() + 18);
    //            DataManager_AllData.Set_ReviveEffectsCount(DataManager_AllData.Get_ReviveEffectsCount() + 30);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_kit_2:
    //            DataManager_AllData.Set_CoinsCount(DataManager_AllData.Get_CoinsCount() + 3550);
    //            DataManager_AllData.Set_ShieldEffectsCount(DataManager_AllData.Get_ShieldEffectsCount() + 8);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_coins:
    //            DataManager_AllData.Set_CoinsCount(DataManager_AllData.Get_CoinsCount() + 1750);
    //            DataManager_AllData.Set_ShieldEffectsCount(DataManager_AllData.Get_ShieldEffectsCount() + 2);
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_player_skin_1_0:
    //            //todo
    //            break;

    //        case EM_IAPConstants.Product_gravity_jump_screen_shop_player_skin_2:
    //            //todo
    //            break;
    //        default:
    //            break;
    //    }

    //    PurchaseEvent?.Invoke(true);
    //    PurchaseEvent = null;
    //}

    //private void PurchaseFailedHandler(IAPProduct product)
    //{
    //    PurchaseEvent?.Invoke(false);
    //    PurchaseEvent = null;
    //}

    //#region МЕТОДЫ ДЛЯ ВОСТАНОВЛЕНИЯ ПОКУПОК
    //public void RestorePurchases()
    //{
    //    InAppPurchasing.RestorePurchases();
    //    // ПОСЛЕ востановнелия покупок для, КАЖДОЙ ПОКУПКИ ДЛЯ НЕЕ ВЫЗЫВАЕТЬСЯ СОБЫТИЕ PurchaseCompletedHandler
    //    // в нём я могу сделать все те действия что нужны при покупке этой покупки
    //}

    //// Successful restoration handler
    //private void RestoreCompletedHandler()
    //{
    //}

    //// Failed restoration handler
    //private void RestoreFailedHandler()
    //{
    //}
    //#endregion
}
