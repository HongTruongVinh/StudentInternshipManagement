using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services.ViewModel
{
    public class ChartItemViewModel
    {
        const string _zeroToFiveColor = "#243e56";
        const string _fiveToSevenColor = "#246b56";
        const string _sevenToEightColor = "#ffff66";
        const string _eightToTenColor = "#ff66ff";


        /// <summary>
        /// màu của danh mục sẽ được hiển thị 
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// Tên danh mục 
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// giá trị 
        /// </summary>
        public float value { get; set; }

        public ChartItemViewModel() { }

        public static List<ChartItemViewModel> CreatePieChartData(List<float?> points)
        {
            var pieChartData = new List<ChartItemViewModel>();

            int numberZeroToFive = 0;
            int numberFiveToSeven = 0;
            int numberSevenToEight = 0;
            int numberEightToTen = 0;


            foreach (var point in points)
            {
                if (point < 5f || point == null)
                {
                    numberZeroToFive++;
                }
                else if (point >= 5f && point < 7f)
                {
                    numberFiveToSeven++;
                }
                else if (point >= 7f && point < 8f)
                {
                    numberSevenToEight++;
                }
                else if (point >= 8f)
                {
                    numberEightToTen++;
                }
            }

            float percentZeroToFive = (float)Math.Round((double)(100 * numberZeroToFive) / points.Count);
            float percentFiveToSeven = (float)Math.Round((double)(100 * numberFiveToSeven) / points.Count);
            float percentSevenToEight = (float)Math.Round((double)(100 * numberSevenToEight) / points.Count);
            float percentEightToTen = (float)Math.Round((double)(100 * numberEightToTen) / points.Count);

            if (percentZeroToFive!=0)
            {
                pieChartData.Add(new ChartItemViewModel() { category = "Điểm 0 -> 5", value = percentZeroToFive, color = _zeroToFiveColor });
            }
            
            if (percentFiveToSeven != 0)
            {
                pieChartData.Add(new ChartItemViewModel() { category = "Điểm 5 -> 7", value = percentFiveToSeven, color = _fiveToSevenColor });
            }
            
            if (percentSevenToEight != 0)
            {
                pieChartData.Add(new ChartItemViewModel() { category = "Điểm 7 -> 8", value = percentSevenToEight, color = _sevenToEightColor });
            }
            
            if (percentEightToTen != 0)
            {
                pieChartData.Add(new ChartItemViewModel() { category = "Điểm 8 -> 10", value = percentEightToTen, color = _eightToTenColor });
            }

            return pieChartData;
        }

        public static List<ChartItemViewModel> CreateBarChartData(List<float?> points)
        {
            var pieChartData = new List<ChartItemViewModel>();

            int numberZeroToFive = 0;
            int numberFiveToSeven = 0;
            int numberSevenToEight = 0;
            int numberEightToTen = 0;


            foreach (var point in points)
            {
                if (point < 5f || point == null)
                {
                    numberZeroToFive++;
                }
                else if (point >= 5f && point < 7f)
                {
                    numberFiveToSeven++;
                }
                else if (point >= 7f && point < 8f)
                {
                    numberSevenToEight++;
                }
                else if (point >= 8f)
                {
                    numberEightToTen++;
                }
            }

            pieChartData.Add(new ChartItemViewModel() { category = "Điểm 0 -> 5", value = numberZeroToFive, color = _zeroToFiveColor });
            pieChartData.Add(new ChartItemViewModel() { category = "Điểm 5 -> 7", value = numberFiveToSeven, color = _fiveToSevenColor });
            pieChartData.Add(new ChartItemViewModel() { category = "Điểm 7 -> 8", value = numberSevenToEight, color = _sevenToEightColor });
            pieChartData.Add(new ChartItemViewModel() { category = "Điểm 8 -> 10", value = numberEightToTen, color = _eightToTenColor });


            return pieChartData;
        }

    }
}
