using System;
using System.Collections.Generic;
using GameSave;
using HarmonyLib;

namespace InvalidGiftsRemover
{
    [Harmony]
    public class Harmony_Patch
    {
        /// <summary>
        /// <see cref="Harmony_Patch"/> の新しいインスタンスを生成します。
        /// </summary>
        public Harmony_Patch()
        {
            try
            {
                var harmony = new Harmony("InvalidGiftsRemover");
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        /// <summary>
        /// <see cref="UnitDataModel.LoadFromSaveData(SaveData)"/> メソッドが呼び出された後に呼び出されます。
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="data"></param>
        [HarmonyPatch(typeof(UnitDataModel), "LoadFromSaveData")]
        [HarmonyPrefix]
        public static bool UnitDataModel_LoadFromSaveData_Prefix(UnitDataModel __instance, SaveData data)
        {
            try
            {
                Log.Instance.InfomationWithCaller("が呼び出されました。");
                Log.Instance.Infomation($"- 対象キャラクター: {{階層: {__instance.OwnerSephirah}, 名前: '{__instance.name}'}}");

                SaveData giftInventoryData = data.GetData(UnitDataModel.save_giftInventory);

                Log.Instance.Infomation("  - 装備中の戦闘表象リストから無効な戦闘表象を削除します。");
                RemoveInvalidGiftId(giftInventoryData.GetData(GiftInventory.save_equipList));

                Log.Instance.Infomation("  - 非装備中の戦闘表象リストから無効な戦闘表象を削除します。");
                RemoveInvalidGiftId(giftInventoryData.GetData(GiftInventory.save_unequipList));
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
            return true;
        }

        private static void RemoveInvalidGiftId(SaveData giftIdListSaveData)
        {
            var removingSaveDataList = new List<SaveData>();
            foreach (SaveData giftIdSaveData in giftIdListSaveData)
            {
                // 戦闘表象XMLデータを取得できる戦闘表象IDは正常データとして読み飛ばす
                if (Singleton<GiftXmlList>.Instance.GetData(giftIdSaveData.GetIntSelf()) != null) { continue; }

                removingSaveDataList.Add(giftIdSaveData);
            }

            if (removingSaveDataList.Count <= 0)
            {
                Log.Instance.Infomation("    - 無効な戦闘表象はありません。");
                return;
            }

            List<SaveData> _list = PrivateAccess.GetField<List<SaveData>>(giftIdListSaveData, "_list");
            foreach (SaveData removingSaveData in removingSaveDataList)
            {
                _list.Remove(removingSaveData);
                Log.Instance.Infomation($"    - 無効な戦闘表象 (ID: {removingSaveData.GetIntSelf()}) を削除しました。");
            }
        }
    }
}
