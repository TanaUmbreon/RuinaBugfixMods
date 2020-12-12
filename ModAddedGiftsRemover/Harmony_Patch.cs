#pragma warning disable CA1031

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;

namespace ModAddedGiftsRemover
{
    /// <summary>
    /// 任意のメソッド呼び出し時にパッチを適用します。
    /// </summary>
    public class Harmony_Patch
    {
        /// <summary>
        /// <see cref="Harmony_Patch"/> の新しいインスタンスを生成します。
        /// </summary>
        public Harmony_Patch()
        {
            try
            {
                new Harmony("ModGiftsRemover").Patch(
                    GetMethod<GiftInventory>("LoadFromSaveData"), null,
                    new HarmonyMethod(GetMethod<Harmony_Patch>("ModGiftsRemover_LoadFromSaveData")), null, null);
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(ex);
            }
        }

        /// <summary>
        /// 指定した型に所属する指定した名前のメソッド情報を取得します。
        /// </summary>
        /// <typeparam name="T">メソッド情報を取得する型。</typeparam>
        /// <param name="name"><typeparamref name="T"/> に所属するメソッドの名前。</param>
        /// <returns></returns>
        private MethodInfo GetMethod<T>(string name) => typeof(T).GetMethod(name, AccessTools.all);

        /// <summary>
        /// <see cref="GiftInventory.LoadFromSaveData"/> メソッドが呼び出された後に時に実行する
        /// <see cref="HarmonyMethod"/> です。
        /// </summary>
        /// <param name="__instance">戦闘表象インベントリ。</param>
        public static void ModGiftsRemover_LoadFromSaveData(GiftInventory __instance)
        {
            var log = new StringBuilder();
            try
            {
                log.AppendLine($"[{nameof(ModGiftsRemover_LoadFromSaveData)}] が呼び出されました。");

                log.AppendLine("<<入手済みの戦闘表象>>");
                __instance.GetAllGiftsListForTitle().ForEach(g => log.AppendLine($"- ID: {g.ClassInfo.id} ({g.GetName()})"));
                
                log.AppendLine("<<削除対象の戦闘表象>>");
                var removingGifts = new List<GiftModel>(GetRemovingGifts(__instance));
                removingGifts.ForEach(g => log.AppendLine($"- ID: {g.ClassInfo.id} ({g.GetName()})"));

                if (removingGifts.Count <= 0)
                {
                    log.AppendLine("削除対象なし。");
                    return;
                }

                foreach (var gift in removingGifts)
                {
                    bool hasUnEquiped = __instance.UnEquip(gift);
                    log.AppendLine($"戦闘表象「{gift.GetName()}」{(hasUnEquiped ? "を外しました。" : "は装備されていません。")}");

                    bool hasDeleted = __instance.GetUnequippedList().Remove(gift);
                    log.AppendLine($"戦闘表象「{gift.GetName()}」{(hasDeleted ? "を削除しました。" : "は削除されています。")}");
                }
            }
            catch (Exception ex)
            {
                log.AppendLine(ex.Message);
                log.AppendLine(ex.StackTrace);
            }
            finally
            {
                Log.Instance.AppendLine(log.ToString());
            }
        }

        /// <summary>
        /// 指定した戦闘表象インベントリから、削除対象の戦闘表象のコレクションを取得します。
        /// </summary>
        /// <param name="__instance"></param>
        /// <returns></returns>
        private static IEnumerable<GiftModel> GetRemovingGifts(GiftInventory __instance)
        {
            const int SavingIdFrom = 0; // 保持する戦闘表象のID値(開始)
            const int SavingIdTo = 47; // 保持する戦闘表象のID値(終了)

            return __instance.GetAllGiftsListForTitle().Where(
                g => g.ClassInfo.id < SavingIdFrom || g.ClassInfo.id > SavingIdTo);
        }
    }
}
