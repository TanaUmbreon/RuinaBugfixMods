using System;
using System.Collections.Generic;

namespace InvalidPassivesRemover
{
    /// <summary>
    /// デバッグをサポートする為の拡張機能を提供します。
    /// </summary>
    internal static class DebugSupportExtensions
    {
        /// <summary>
        /// 各要素のフィールドの値を文字列として列挙します。
        /// </summary>
        /// <param name="passive">文字列として列挙するパッシブのコレクションのインスタンス。</param>
        /// <returns>文字列として表された各要素のフィールドの値のコレクション。</returns>
        public static IEnumerable<string> EnumerateItemFieldValue(this IEnumerable<PassiveModel> passives)
        {
            int idx = 0;
            foreach (var passive in passives)
            {
                yield return $"- **#{idx}**";
                foreach (string value in passive.EnumerateFieldValue())
                {
                    yield return  $"  {value}";
                }
                idx++;
            }
        }

        /// <summary>
        /// フィールドの値を文字列として列挙します。
        /// </summary>
        /// <param name="passive">文字列として列挙するパッシブのインスタンス。</param>
        /// <returns>文字列として表されたフィールドの値のコレクション。</returns>
        public static IEnumerable<string> EnumerateFieldValue(this PassiveModel passive)
        {
            if (passive == null) { throw new ArgumentNullException(nameof(passive)); }

            yield return $"- `isAddedPassive`: {passive.isAddedPassive}";
            yield return $"- `originData`: {passive.originData?.ToString() ?? "null"}";
            if (passive.originData != null)
            {
                foreach (string value in passive.originData.EnumerateFieldValue())
                {
                    yield return $"  {value}";
                }
            }
            yield return $"- `originpassive`: {passive.originpassive?.ToString() ?? "null"}";
            if (passive.originpassive != null)
            {
                foreach (string value in passive.originpassive.EnumerateFieldValue())
                {
                    yield return $"  {value}";
                }
            }
            yield return $"- `reservedData`: {passive.reservedData?.ToString() ?? "null"}";
            if (passive.reservedData != null)
            {
                foreach (string value in passive.reservedData.EnumerateFieldValue())
                {
                    yield return $"  {value}";
                }
            }
        }

        /// <summary>
        /// フィールドの値を文字列として列挙します。
        /// </summary>
        /// <param name="data">文字列として列挙するパッシブのセーブ データのインスタンス。</param>
        /// <returns>文字列として表されたフィールドの値のコレクション。</returns>
        public static IEnumerable<string> EnumerateFieldValue(this PassiveModel.PassiveModelSavedData data)
        {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            yield return $"- `currentpassive`: {data.currentpassive?.ToString() ?? "null"}";
            if (data.currentpassive != null)
            {
                foreach (string value in data.currentpassive.EnumerateFieldValue())
                {
                    yield return $"  {value}";
                }
            }
            yield return $"- `givePassiveBookId`: {data.givePassiveBookId}";
            yield return $"- `receivepassivebookId`: {data.receivepassivebookId}";
        }

        /// <summary>
        /// フィールドの値を文字列として列挙します。
        /// </summary>
        /// <param name="info">文字列として列挙するパッシブ情報のインスタンス。</param>
        /// <returns>文字列として表されたフィールドの値のコレクション。</returns>
        public static IEnumerable<string> EnumerateFieldValue(this PassiveXmlInfo info)
        {
            if (info == null) { throw new ArgumentNullException(nameof(info)); }

            yield return $"- `CanGivePassive`: {info.CanGivePassive}";
            yield return $"- `CanReceivePassive`: {info.CanReceivePassive}";
            yield return $"- `cost`: {info.cost}";
            yield return $"- `EmotionCondition`: {info.EmotionCondition}";
            yield return $"- `id`: {info.id}";
            yield return $"- `InnerTypeId`: {info.InnerTypeId}";
            yield return $"- `isHide`: {info.isHide}";
            yield return $"- `isLock`: {info.isLock}";
            yield return $"- `isNegative`: {info.isNegative}";
            yield return $"- `level`: {info.level}";
            yield return $"- `needlevel`: {info.needlevel}";
            yield return $"- `param`: {info.param?.ToString() ?? "null"}";
            yield return $"- `rare`: {info.rare}";
            yield return $"- `releaseLevel`: {info.releaseLevel}";
        }
    }
}