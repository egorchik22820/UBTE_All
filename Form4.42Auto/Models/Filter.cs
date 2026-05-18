using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form4._42Auto.Models
{
    public static class Filter
    {

        public static List<ExportDataTable> PromOrgFilter_GCal(List<ExportDataTable> dataList) // для данной группы только в Гкал убирать  объем услуг по передаче
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000006 3. Промышленные и приравненные к ним" &&
            x.CounterAgentExpenseType != "Услуги по передаче тепла").ToList();
        }
        public static List<ExportDataTable> PromOrgFilter(List<ExportDataTable> dataList)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000006 3. Промышленные и приравненные к ним").ToList();
        }

        public static List<ExportDataTable>MinOboronOrgFilter(List<ExportDataTable> dataList)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000007 4. Бюджетные потребители" &&
            x.Classifier_9_2 == "А0000000033 4.1. Федеральный бюджет" &&
            !string.IsNullOrEmpty(x.Classifier_9_5)).ToList();
        }

        public static List<ExportDataTable> FromFedBujetFilter(List<ExportDataTable> dataList)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000007 4. Бюджетные потребители" &&
            x.Classifier_9_2 == "А0000000033 4.1. Федеральный бюджет" &&
            string.IsNullOrEmpty(x.Classifier_9_5)).ToList();
        }

        public static List<ExportDataTable> FromSubFedBujetFilter(List<ExportDataTable> dataList)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000007 4. Бюджетные потребители" &&
            (x.Classifier_9_2 == "А0000000034 4.2. Региональный бюджет" ||
            x.Classifier_9_2 == "А0000000035 4.3. Местный бюджет")).ToList();
        }

        public static List<ExportDataTable> NaselenFilter(List<ExportDataTable> dataList)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000001 1. Население" ||
            x.Classifier_9_1 == "А0000000005 2. Исполнители коммунальных услуг").ToList();
        }

        public static List<ExportDataTable> TeploOrgFilter(List<ExportDataTable> dataList)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000008 5. Теплоснабжающие и теплосетевые организации").ToList();
        }

        public static List<ExportDataTable> ProchFilter(List<ExportDataTable> dataList) // без ВГО, без ХозНужды (в контрагентах  Т Плюс, ПАО)
        {
            return dataList.Where(x =>
            x.Classifier_9_1 == "А0000000009 6. Прочие потребители" &&
            x.CounterAgent != "Т Плюс, ПАО" &&
            x.IsVGO == "<Не ВГО>").ToList();
        }
    }
}
