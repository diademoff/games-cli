using System;

namespace Games
{
    /// <summary>
    /// Параметры конфигурации с выбором
    /// </summary>
    public class ConfigParam
    {
        public string ConfigTitle;
        public string[] PossibleValues;
        public string SelectedValue => PossibleValues[SelectedValueIndex];
        public int SelectedValueIndex
        {
            get
            {
                return selectedValue;
            }
            set
            {
                if (value < 0 || value >= PossibleValues.Length)
                    throw new IndexOutOfRangeException();
                selectedValue = value;
                OnValueChanged?.Invoke(PossibleValues[value]);
            }
        }
        int selectedValue;
        public Action<string> OnValueChanged;

        public ConfigParam(string title, string[] values)
        {
            this.ConfigTitle = title;
            this.PossibleValues = values;
            this.SelectedValueIndex = 0;
        }

        /// <summary>
        /// IConfigValue - это ссылочный тип, поэтому возможно изменение
        /// конфигурации при вызове OnValueChanged
        /// </summary>
        public ConfigParam BindTo<T>(IConfigValue<T> v)
        {
            // Выбрать пункт, который сейчас находится в конфиге
            var i = Array.IndexOf(PossibleValues, v.Value.ToString());
            i = i == -1 ? 0 : i;

            this.SelectedValueIndex = i;
            this.OnValueChanged += (s) => v.ChangeValueTo(s);
            return this;
        }

        public string SelectNextValue()
        {
            if (SelectedValueIndex == PossibleValues.Length - 1)
                SelectedValueIndex = 0;
            else
                SelectedValueIndex++;
            return this.ToString();
        }

        public string SelectPrevValue()
        {
            if (SelectedValueIndex == 0)
                SelectedValueIndex = PossibleValues.Length - 1;
            else
                SelectedValueIndex--;
            return this.ToString();
        }

        public override string ToString()
        {
            return $"{this.ConfigTitle} [{this.PossibleValues[SelectedValueIndex]}]";
        }
    }
}