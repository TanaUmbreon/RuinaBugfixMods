using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace InvalidPassivesRemover
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
                var harmony = new Harmony("InvalidPassivesRemover");
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        /// <summary>
        /// <see cref="BookModel.LoadFromSaveData"/> メソッドが呼び出された後に時に呼び出されます。
        /// </summary>
        /// <param name="__instance">ページのインスタンス。</param>
        [HarmonyPatch(typeof(BookModel), "LoadFromSaveData")]
        [HarmonyPostfix]
        public static void BookModel_LoadFromSaveData_Postfix(BookModel __instance)
        {
            try
            {
                Log.Instance.InfomationWithCaller($"が呼び出されました。");
                Log.Instance.Infomation($"ページID: {__instance.instanceId} ({__instance.Name})");

                var invalidPassives = new List<PassiveModel>();

                Log.Instance.Infomation("<<無効なパッシブの検知 (装備中のパッシブ)>>");
                int slotIndex = 0;
                foreach (PassiveModel passive in __instance.GetPassiveModelList())
                { 
                    // いずれかのPassiveXmlInfoが定義されていないものを無効なパッシブと判断する
                    if (passive.originpassive == null || passive.originData.currentpassive == null)
                    {
                        invalidPassives.Add(passive);
                        Log.Instance.Infomation($"- スロット {slotIndex}: null (無効なパッシブ)");
                    }
                    else
                    {
                        Log.Instance.Infomation($"- スロット {slotIndex}: {passive.originData.currentpassive.id}");
                    }

                    slotIndex++;
                }

                if (invalidPassives.Any())
                {
                    Log.Instance.Infomation("無効なパッシブを検出しました。装備中のパッシブを初期化します。");
                    InitializeInvalidPassives(__instance, invalidPassives);
                    Log.Instance.Infomation("装備中のパッシブを初期化しました。");
                }
                else
                {
                    Log.Instance.Infomation("無効なパッシブはありません。");
                }

                Log.Instance.Infomation("<<装備中のパッシブ>>");
                __instance.GetPassiveInfoList().ForEach(p => Log.Instance.Infomation($"- ID: {p.passive.id} ({p.name})"));
            }
            catch (Exception ex)
            {
                Log.Instance.ErrorOnExceptionThrown(ex);
            }
        }

        /// <summary>
        /// 指定したページから指定した無効なパッシブを初期化します。
        /// </summary>
        /// <param name="book"></param>
        /// <param name="invalidPassives"></param>
        /// <param name="log"></param>
        private static void InitializeInvalidPassives(BookModel book, IEnumerable<PassiveModel> invalidPassives)
        {
            Type type = book.GetType();
            FieldInfo field = type.GetField("_activatedAllPassives", BindingFlags.NonPublic | BindingFlags.Instance);
            if (!(field.GetValue(book) is List<PassiveModel> activatedAllPassives))
            {
                throw new ArgumentNullException("BookModel._activatedAllPassives が取得できません。");
            }

            //// デバッグ用途出力
            //Log.Instance.Infomation("activatedAllPassives の値");
            //foreach (string value in activatedAllPassives.EnumerateItemFieldValue())
            //{
            //    Log.Instance.Infomation(value);
            //}

            // 無効なパッシブは空のスロットに置き換える
            foreach (PassiveModel passive in invalidPassives)
            {
                int index = activatedAllPassives.IndexOf(passive);
                if (index < 0) { continue; }

                activatedAllPassives[index] = new PassiveModel(new LorId(0), book.instanceId, 1);
                Log.Instance.Infomation($"スロット {index} のパッシブを初期化しました。");
            }

            book.TryGainUniquePassive();
        }
    }
}
