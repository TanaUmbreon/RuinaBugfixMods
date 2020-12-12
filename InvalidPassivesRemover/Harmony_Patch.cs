#pragma warning disable CA1031

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameSave;
using HarmonyLib;
using UI;

namespace InvalidPassivesRemover
{
    public class Harmony_Patch
    {
        /// <summary>
        /// <see cref="Harmony_Patch"/> の新しいインスタンスを生成します。
        /// </summary>
        public Harmony_Patch()
        {
            try
            {
                new Harmony("InvalidPassivesRemover").Patch(
                    GetMethod<BookModel>("LoadFromSaveData"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>("InvalidPassivesRemover_LoadFromSaveData")), null, null);

                new Harmony("InvalidPassivesRemover").Patch(
                    GetMethod<UIPassiveSuccessionPopup>("OnClickReleaseAllEquipedBookButton"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>(nameof(UIPassiveSuccessionPopup_OnClickReleaseAllEquipedBookButton))), null, null);

                new Harmony("InvalidPassivesRemover").Patch(
                    GetMethod<UILibrarianInfoPanel>("OnClickReleaseBookButton"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>(nameof(UILibrarianInfoPanel_OnClickReleaseBookButton))), null, null);

                new Harmony("InvalidPassivesRemover").Patch(
                    GetMethod<UIPassiveSuccessionCenterEquipBookSlot>("OnClickReleaseButton"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>(nameof(UIPassiveSuccessionCenterEquipBookSlot_OnClickReleaseButton))), null, null);

                new Harmony("InvalidPassivesRemover").Patch(
                    GetMethod<UILibrarianInfoInCardPhase>("OnClickReleaseToggle"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>(nameof(UILibrarianInfoInCardPhase_OnClickReleaseToggle))), null, null);

                new Harmony("InvalidPassivesRemover").Patch(
                    GetMethod<UIBattleSettingLibrarianInfoPanel>("OnClickReleaseToggle"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>(nameof(UIBattleSettingLibrarianInfoPanel_OnClickReleaseToggle))), null, null);
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(ex);
            }
        }

        public static void UIPassiveSuccessionPopup_OnClickReleaseAllEquipedBookButton(UIPassiveSuccessionPopup __instance) =>
            Log.Instance.AppendLine($"Called {nameof(UIPassiveSuccessionPopup_OnClickReleaseAllEquipedBookButton)}");
        public static void UILibrarianInfoPanel_OnClickReleaseBookButton(UILibrarianInfoPanel __instance) =>
            Log.Instance.AppendLine($"Called {nameof(UILibrarianInfoPanel_OnClickReleaseBookButton)}");
        public static void UIPassiveSuccessionCenterEquipBookSlot_OnClickReleaseButton(UIPassiveSuccessionCenterEquipBookSlot __instance) =>
            Log.Instance.AppendLine($"Called {nameof(UIPassiveSuccessionCenterEquipBookSlot_OnClickReleaseButton)}");
        public static void UILibrarianInfoInCardPhase_OnClickReleaseToggle(UILibrarianInfoInCardPhase __instance) =>
            Log.Instance.AppendLine($"Called {nameof(UILibrarianInfoInCardPhase_OnClickReleaseToggle)}");
        public static void UIBattleSettingLibrarianInfoPanel_OnClickReleaseToggle(UIBattleSettingLibrarianInfoPanel __instance) =>
            Log.Instance.AppendLine($"Called {nameof(UIBattleSettingLibrarianInfoPanel_OnClickReleaseToggle)}");


        /// <summary>
        /// 指定した型に所属する指定した名前のメソッド情報を取得します。
        /// </summary>
        /// <typeparam name="T">メソッド情報を取得する型。</typeparam>
        /// <param name="name"><typeparamref name="T"/> に所属するメソッドの名前。</param>
        /// <returns></returns>
        private MethodInfo GetMethod<T>(string name) => typeof(T).GetMethod(name, AccessTools.all);

        /// <summary>
        /// <see cref="BookModel.LoadFromSaveData"/> メソッドが呼び出された後に時に実行する
        /// <see cref="HarmonyMethod"/> です。
        /// </summary>
        /// <param name="__instance">コア ページのインスタンス。</param>
        public static void InvalidPassivesRemover_LoadFromSaveData(BookModel __instance)
        {
            var log = new StringBuilder();
            try
            {
                log.AppendLine($"[{nameof(InvalidPassivesRemover_LoadFromSaveData)}] が呼び出されました。");
                log.AppendLine($"コアページID: {__instance.instanceId} ({__instance.Name})");

                log.AppendLine("<<SaveData>>");
                SaveData data = __instance.GetSaveData();
                SaveData data4 = data.GetData("allpassiveList");
                if (data4 != null)
                {
                    foreach (SaveData data5 in data4)
                    {
                        PassiveModel passiveModel = new PassiveModel(__instance.instanceId);
                        passiveModel.LoadFromSaveData(data5);
                        log.AppendLine($"- Id: {passiveModel?.originData?.currentpassive?.id.ToString() ?? "null (passiveModel.originData.currentpassive <PassiveXmlInfo> が null)"}");
                    }
                }

                log.AppendLine("<<装備中のパッシブ>>");
                __instance.GetPassiveInfoList().ForEach(p => log.AppendLine($"- ID: {p.passive.id} ({p.name})"));

                log.AppendLine("<<帰属中のコアページ>>");
                __instance.GetEquipedBookList().ForEach(b => log.AppendLine($"- ID: {b.instanceId} ({b.Name})"));
            }
            catch (Exception ex)
            {
                log.AppendLine("データを取得できませんでした。");
                log.AppendLine(ex.Message);
                log.AppendLine(ex.StackTrace);

                try
                {
                    ReleaseAllEquipedBooks(__instance);
                    log.AppendLine("ReleaseAllEquipedBooksを呼び出しました。");

                    log.AppendLine("<<装備中のパッシブ>>");
                    __instance.GetPassiveInfoList().ForEach(p => log.AppendLine($"- ID: {p.passive.id} ({p.name})"));
                }
                catch (Exception)
                {
                    log.AppendLine("データを取得できませんでした。");
                    log.AppendLine(ex.Message);
                    log.AppendLine(ex.StackTrace);
                }
            }
            finally
            {
                //Log.Instance.AppendLine(log.ToString());
            }
        }

        /// <summary>
        /// 指定したコア ページの帰属を初期化します。
        /// </summary>
        private static void ReleaseAllEquipedBooks(BookModel book)
        {
            // UI.UIInvenLeftEquipPageSlot.OnClickRelaseButton() メソッドと同等の内容

            if (book.CanEquipBookByGivePassive())
            {
                book.ReleaseAllEquipedPassiveBooks(true);
                return;
            }

            BookModel bookByInstanceIdInAllBookEquiped =
                Singleton<BookInventoryModel>.Instance.GetBookByInstanceIdInAllBookEquiped(
                    book.originData.equipedPassiveBookInstanceId, book.instanceId, true);

            if (bookByInstanceIdInAllBookEquiped == null)
            {
                Log.Instance.AppendLine($"ID: {book.originData.equipedPassiveBookInstanceId} の本が存在しません。");
                return;
            }

            bookByInstanceIdInAllBookEquiped.UnEquipGivePassiveBook(book, true);
        }
    }
}
