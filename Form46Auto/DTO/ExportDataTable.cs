using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form46Auto.DTO
{
    public class ExportDataTable
    {
        // Классификатор договора
        public string ContractClassifier { get; set; }

        // Номенклатура
        public string Nomenclature { get; set; }

        // Номенклатура.Единица
        public string NomenclatureUnit { get; set; }

        // Тариф
        public string Tariff { get; set; }

        // Количество
        public decimal Quantity { get; set; }

        // Количество по прибору
        public decimal QuantityByMeter { get; set; }

        // Сумма
        public decimal Amount { get; set; }

        // Сумма по прибору
        public decimal AmountByMeter { get; set; }

        // Сумма без НДС
        public decimal AmountWithoutVAT { get; set; }

        // Теплоисточник
        public string HeatSource { get; set; }

        // Вид нагрузки
        public string LoadType { get; set; }

        // Подразделение
        public string Department { get; set; }

        // Бизнес метод расчета
        public string BusinessCalculationMethod { get; set; }

        // Год перерасчета
        public decimal RecalculationYear { get; set; }

        // Конструктор для удобства инициализации
        public ExportDataTable()
        {
        }

        // Конструктор для удобного создания объекта
        public ExportDataTable(
            string contractClassifier,
            string nomenclature,
            string tariff,
            string nomenclatureUnit,
            decimal quantity,
            decimal quantityByMeter,
            decimal amount,
            decimal amountByMeter,
            decimal amountWithoutVAT,
            string heatSource,
            string loadType,
            string department,
            string businessCalculationMethod,
            decimal recalculationYear)
        {
            ContractClassifier = contractClassifier;
            Nomenclature = nomenclature;
            NomenclatureUnit = nomenclatureUnit;
            Tariff = tariff;
            Quantity = quantity;
            QuantityByMeter = quantityByMeter;
            Amount = amount;
            AmountByMeter = amountByMeter;
            AmountWithoutVAT = amountWithoutVAT;
            HeatSource = heatSource;
            LoadType = loadType;
            Department = department;
            BusinessCalculationMethod = businessCalculationMethod;
            RecalculationYear = recalculationYear;
        }

        public override string ToString()
        {
            return $"Подразделение: {Department}, Год: {RecalculationYear}, Ном:\t{Nomenclature}, Классификация:\t{ContractClassifier}";
        }
    }

}
