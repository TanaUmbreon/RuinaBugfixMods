#pragma warning disable CA1031

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;

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
                PatchPostfixHarmonyMethod<BookModel>(
                    originalMethodName: "LoadFromSaveData",
                    harmonyMethodName: nameof(InvalidPassivesRemover_LoadFromSaveData));
            }
            catch (Exception ex)
            {
                Log.Instance.AppendLine(ex);
            }
        }

        /// <summary>
        /// 型パラメータに指定した型に所属する指定した名前のメソッド (オリジナル メソッド) が呼び出された後、
        /// 指定した名前のメソッドを呼び出す HarmonyPatch を適用します。
        /// </summary>
        /// <typeparam name="T">オリジナル メソッドが所属する型。</typeparam>
        /// <param name="name"><typeparamref name="T"/> に所属するメソッドの名前。</param>
        /// <param name="harmonyMethodName">オリジナルのメソッドが呼び出された後に呼び出されるメソッドの名前。</param>
        private void PatchPostfixHarmonyMethod<T>(string originalMethodName, string harmonyMethodName)
        {
            new Harmony("InvalidPassivesRemover").Patch(
                typeof(T).GetMethod(originalMethodName, AccessTools.all), 
                null,
                new HarmonyMethod(typeof(Harmony_Patch).GetMethod(harmonyMethodName, AccessTools.all)), 
                null, 
                null);
        }


        /// <summary>
        /// <see cref="BookModel.LoadFromSaveData"/> メソッドが呼び出された後に時に実行する
        /// <see cref="HarmonyMethod"/> です。
        /// </summary>
        /// <param name="__instance">ページのインスタンス。</param>
        public static void InvalidPassivesRemover_LoadFromSaveData(BookModel __instance)
        {
            var log = new StringBuilder();
            try
            {
                log.AppendLine($"[{nameof(InvalidPassivesRemover_LoadFromSaveData)}] が呼び出されました。");
                log.AppendLine($"ページID: {__instance.instanceId} ({__instance.Name})");

                var invalidPassives = new List<PassiveModel>();

                log.AppendLine("<<無効なパッシブの検知 (装備中のパッシブ)>>");
                int slotIndex = 0;
                foreach (PassiveModel passive in __instance.GetPassiveModelList())
                { 
                    // いずれかのPassiveXmlInfoが定義されていないものを無効なパッシブと判断する
                    if (passive.originpassive == null || passive.originData.currentpassive == null)
                    {
                        invalidPassives.Add(passive);
                        log.AppendLine($"- スロット {slotIndex}: null (無効なパッシブ)");
                    }
                    else
                    {
                        log.AppendLine($"- スロット {slotIndex}: {passive.originData.currentpassive.id}");
                    }

                    slotIndex++;
                }

                if (invalidPassives.Any())
                {
                    log.AppendLine("無効なパッシブを検出しました。装備中のパッシブを初期化します。");
                    InitializeInvalidPassives(__instance, invalidPassives, log);
                    log.AppendLine("装備中のパッシブを初期化しました。");
                }
                else
                {
                    log.AppendLine("無効なパッシブはありません。");
                }

                log.AppendLine("<<装備中のパッシブ>>");
                __instance.GetPassiveInfoList().ForEach(p => log.AppendLine($"- ID: {p.passive.id} ({p.name})"));
            }
            catch (Exception ex)
            {
                log.AppendLine("データを取得できませんでした。");
                log.AppendLine(ex.Message);
                log.AppendLine(ex.GetType().FullName);
                log.AppendLine(ex.StackTrace);
            }
            finally
            {
                Log.Instance.AppendLine(log.ToString());
            }
        }

        /// <summary>
        /// 指定したページから指定した無効なパッシブを初期化します。
        /// </summary>
        /// <param name="book"></param>
        /// <param name="invalidPassives"></param>
        /// <param name="log"></param>
        private static void InitializeInvalidPassives(BookModel book, IEnumerable<PassiveModel> invalidPassives, StringBuilder log)
        {
            Type type = book.GetType();
            FieldInfo field = type.GetField("_activatedAllPassives", BindingFlags.NonPublic | BindingFlags.Instance);
            if (!(field.GetValue(book) is List<PassiveModel> activatedAllPassives))
            {
                throw new ArgumentNullException("BookModel._activatedAllPassives が取得できません。");
            }

            //// デバッグ用途出力
            //log.AppendLine("activatedAllPassives の値");
            //foreach (string value in activatedAllPassives.EnumerateItemFieldValue())
            //{
            //    log.AppendLine(value);
            //}

            // 無効なパッシブは空のスロットに置き換える
            foreach (PassiveModel passive in invalidPassives)
            {
                int index = activatedAllPassives.IndexOf(passive);
                if (index < 0) { continue; }

                activatedAllPassives[index] = new PassiveModel(0, book.instanceId, 1);
                log.AppendLine($"スロット {index} のパッシブを初期化しました。");
            }

            book.TryGainUniquePassive();
        }
    }
}
