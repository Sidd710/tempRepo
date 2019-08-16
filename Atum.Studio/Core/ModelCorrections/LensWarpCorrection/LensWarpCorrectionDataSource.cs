using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.ModelCorrections.LensWarpCorrection
{
    public class LensWarpCorrectionItem
    {
        public int Number { get; set; }
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
    }

    public class LensWarpCorrectionItems : BindingList<LensWarpCorrectionItem>
    {
        public static LensWarpCorrectionItems FromPrinterSettings(DAL.Hardware.AtumPrinter selectedPrinter)
        {
            if (selectedPrinter.LensWarpingCorrection.HorizontalValues == null)
            {
                selectedPrinter.LensWarpingCorrection.CreateDefaultValues();
            }

            var lensWarpCorrectionItems = new LensWarpCorrectionItems();

            if (selectedPrinter.LensWarpingCorrection != null && selectedPrinter.LensWarpingCorrection.HorizontalValues != null)
            {
                for (var itemRowIndex = 0; itemRowIndex < 8; itemRowIndex++)
                {
                    for (var itemColumnIndex = 0; itemColumnIndex < 8; itemColumnIndex++)
                    {
                        lensWarpCorrectionItems.Add(new LensWarpCorrectionItem()
                        {
                            Number = (itemRowIndex * 8) + itemColumnIndex + 1,
                            Horizontal = selectedPrinter.LensWarpingCorrection.HorizontalValues[itemRowIndex][itemColumnIndex],
                            Vertical = selectedPrinter.LensWarpingCorrection.VerticalValues[itemRowIndex][itemColumnIndex],
                        });
                    }
                }
            }

            return lensWarpCorrectionItems;
        }

        public static void ToPrinterSettings(LensWarpCorrectionItems lensWarpCorrectionItems, DAL.Hardware.AtumPrinter selectedPrinter)
        {
            selectedPrinter.LensWarpingCorrection.HorizontalValues = new List<List<float>>();
            selectedPrinter.LensWarpingCorrection.VerticalValues = new List<List<float>>();

            for (var itemRowIndex = 0; itemRowIndex < 8; itemRowIndex++)
            {
                var horizontalValues = new List<float>();
                var verticalValues = new List<float>();

                for (var itemColumnIndex = 0; itemColumnIndex < 8; itemColumnIndex++)
                {
                    var rowIndex = (itemRowIndex * 8) + itemColumnIndex;
                    horizontalValues.Add(lensWarpCorrectionItems[rowIndex].Horizontal);
                    verticalValues.Add(lensWarpCorrectionItems[rowIndex].Vertical);
                }

                selectedPrinter.LensWarpingCorrection.HorizontalValues.Add(horizontalValues);
                selectedPrinter.LensWarpingCorrection.VerticalValues.Add(verticalValues);
            }
        }
    }
}
