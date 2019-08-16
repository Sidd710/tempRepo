using System;
using System.Collections.Generic;
using System.Linq;

namespace Atum.DAL.Catalogs
{
    public enum TypeOfTechnicalSpecificationOrResinProperty
    {
        CuringWaveLength = 10,
        LiquidDensity = 11,
        Viscosity = 12,
        Hardness = 13,
        ResidualAshContent = 14,
        Shrinkage = 15,
        MedicalClass = 16,
        TensileStrength = 17,
        ElongationAtBreak = 18,
        TensileModulus = 19,
        FlexuralStrength = 20,
        FlexuralModulus = 21,
        Appearance = 22,
    }

    [Serializable]
    public class MaterialTechnicalSpecificationsAndResinProperty
    {
        public TypeOfTechnicalSpecificationOrResinProperty TechnicalSpecificationOrResinProperty { get; set; }
        public string Value { get; set; }
        public string ValueSuffix { get; set; }

        public string DisplayName {
            get
            {
               switch(this.TechnicalSpecificationOrResinProperty)
                {
                    case TypeOfTechnicalSpecificationOrResinProperty.Appearance:
                        return "Appearance";
                    case TypeOfTechnicalSpecificationOrResinProperty.CuringWaveLength:
                        return "Curing wave length";
                    case TypeOfTechnicalSpecificationOrResinProperty.ElongationAtBreak:
                        return "Elongation at break";
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralModulus:
                        return "Flexural modulus";
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralStrength:
                        return "Flexural strength";
                    case TypeOfTechnicalSpecificationOrResinProperty.Hardness:
                        return "Hardness";
                    case TypeOfTechnicalSpecificationOrResinProperty.LiquidDensity:
                        return "Liquid density";
                    case TypeOfTechnicalSpecificationOrResinProperty.MedicalClass:
                        return "Medical class";
                    case TypeOfTechnicalSpecificationOrResinProperty.ResidualAshContent:
                        return "Residual ash content";
                    case TypeOfTechnicalSpecificationOrResinProperty.Shrinkage:
                        return "Shrinkage";
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileModulus:
                        return "Tensile modulus";
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileStrength:
                        return "Tensile strength";
                    case TypeOfTechnicalSpecificationOrResinProperty.Viscosity:
                        return "Viscosity";
                }

                return TechnicalSpecificationOrResinProperty.ToString();
            }
        }

        public string DisplayNameShort
        {
            get
            {
                switch (this.TechnicalSpecificationOrResinProperty)
                {
                    case TypeOfTechnicalSpecificationOrResinProperty.Appearance:
                        return "Appearance";
                    case TypeOfTechnicalSpecificationOrResinProperty.CuringWaveLength:
                        return "Curing wave";
                    case TypeOfTechnicalSpecificationOrResinProperty.ElongationAtBreak:
                        return "El. at break";
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralModulus:
                        return "Flex. modulus";
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralStrength:
                        return "Flex. strength";
                    case TypeOfTechnicalSpecificationOrResinProperty.Hardness:
                        return "Hardness";
                    case TypeOfTechnicalSpecificationOrResinProperty.LiquidDensity:
                        return "Liq. density";
                    case TypeOfTechnicalSpecificationOrResinProperty.MedicalClass:
                        return "Med. Class";
                    case TypeOfTechnicalSpecificationOrResinProperty.ResidualAshContent:
                        return "Res. ash con.";
                    case TypeOfTechnicalSpecificationOrResinProperty.Shrinkage:
                        return "Shrinkage";
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileModulus:
                        return "Tens. modulus.";
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileStrength:
                        return "Tens. strength";
                    case TypeOfTechnicalSpecificationOrResinProperty.Viscosity:
                        return "Viscosity";
                }

                return TechnicalSpecificationOrResinProperty.ToString();
            }
        }

        public string DisplayValueShort
        {
            get
            {
                switch (this.TechnicalSpecificationOrResinProperty)
                {
                    case TypeOfTechnicalSpecificationOrResinProperty.Appearance:
                        return string.Format("{0}", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.CuringWaveLength:
                        return string.Format("{0} nm", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.ElongationAtBreak:
                        return string.Format("{0} %", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralModulus:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralStrength:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.Hardness:
                        return string.Format("{0}", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.LiquidDensity:
                        return string.Format("{0} g/cm3", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.MedicalClass:
                        return string.Format("{0}", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.ResidualAshContent:
                        return string.Format("{0} %", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.Shrinkage:
                        return string.Format("{0} %", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileModulus:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileStrength:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.Viscosity:
                        return string.Format("{0}", this.Value);
                }

                return TechnicalSpecificationOrResinProperty.ToString();
            }
        }

        public string DisplayValue
        {
            get
            {
                switch (this.TechnicalSpecificationOrResinProperty)
                {
                    case TypeOfTechnicalSpecificationOrResinProperty.Appearance:
                        return string.Format("{0}", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.CuringWaveLength:
                        return string.Format("{0} nm", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.ElongationAtBreak:
                        return string.Format("{0} %", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralModulus:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.FlexuralStrength:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.Hardness:
                        return string.Format("{0}", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.LiquidDensity:
                        return string.Format("{0} g/cm3", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.MedicalClass:
                        return string.Format("{0}", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.ResidualAshContent:
                        return string.Format("{0} %", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.Shrinkage:
                        return string.Format("{0} %", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileModulus:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.TensileStrength:
                        return string.Format("{0} Mpa", this.Value);
                    case TypeOfTechnicalSpecificationOrResinProperty.Viscosity:
                        return string.Format("{0} mPa.s @{1}°C", this.Value, this.ValueSuffix);
                }

                return TechnicalSpecificationOrResinProperty.ToString();
            }
        }

        public MaterialTechnicalSpecificationsAndResinProperty()
        {

        }

        public MaterialTechnicalSpecificationsAndResinProperty(TypeOfTechnicalSpecificationOrResinProperty techSpecType, string value, string valueSuffix = "")
        {
            this.TechnicalSpecificationOrResinProperty = techSpecType;
            this.Value = value;
            this.ValueSuffix = valueSuffix;
        }
    }

    [Serializable]
    public class MaterialTechnicalSpecificationsAndResinProperties: List<MaterialTechnicalSpecificationsAndResinProperty>
    {
        public MaterialTechnicalSpecificationsAndResinProperty ByType(TypeOfTechnicalSpecificationOrResinProperty property)
        {
            return this.First(s => s.TechnicalSpecificationOrResinProperty == property);
        }
    }
}
