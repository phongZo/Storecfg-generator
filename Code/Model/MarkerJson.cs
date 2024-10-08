// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.MarkerJson
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Text.RegularExpressions;

namespace StorecfgGenerator
{
    public class MarkerJson
    {
        private float opacity = 0.04f;
        private float gridOpacity = 1f;
        private int cols = 6;
        private int rows = 3;
        private float cellWidth = 15f;
        private float cellHeight = 15f;
        private int gridSpacingX = 500;
        private int gridSpacingY = 500;
        private int randomOffsetDistance = 0;
        private float marginPercent = 0.15f;
        private string gridColor1 = "#ffffff";
        private string gridColor2 = "#000000";
        private int textSize = 25;
        private int textAngle = 45;
        private int textSpacingX = 550;
        private int textSpacingY = 550;
        private string textColor1 = "#000000";
        private string textColor2 = "#333333";

        public bool DrawingEnabled { get; set; } = false;

        public float Opacity
        {
            get => this.opacity;
            set => this.opacity = value;
        }

        public string TimestampFormat { get; set; } = "ddMMyyyy HH:mm";

        public bool GridEnabled { get; set; } = false;

        public string GridShape { get; set; } = "Circle";

        public float GridBlurRadius { get; set; } = 0.0f;

        public string GridLogoURL { get; set; } = "https://assets.agilemark.io/images/agilemark-logo.png";

        public string GridLogoHash { get; set; } = "";

        public string GridSecondaryLogoURL { get; set; } = "https://assets.agilemark.io/images/agilemark-logo.png";

        public string GridSecondaryLogoHash { get; set; } = "";

        public bool GridShowTimestamp { get; set; } = false;

        public float GridOpacity
        {
            get => this.gridOpacity;
            set => this.gridOpacity = value;
        }

        public int Cols
        {
            get => this.cols;
            set => this.cols = value;
        }

        public int Rows
        {
            get => this.rows;
            set => this.rows = value;
        }

        public float CellWidth
        {
            get => this.cellWidth;
            set => this.cellWidth = value;
        }

        public float CellHeight
        {
            get => this.cellHeight;
            set => this.cellHeight = value;
        }

        public int GridSpacingX
        {
            get => this.gridSpacingX;
            set => this.gridSpacingX = value;
        }

        public int GridSpacingY
        {
            get => this.gridSpacingY;
            set => this.gridSpacingY = value;
        }

        public int RandomOffsetDistance
        {
            get => this.randomOffsetDistance;
            set => this.randomOffsetDistance = value;
        }

        public float MarginPercent
        {
            get => this.marginPercent;
            set => this.marginPercent = value;
        }

        public string GridColor1
        {
            get => this.gridColor1;
            set => this.gridColor1 = value;
        }

        public string GridColor2
        {
            get => this.gridColor2;
            set => this.gridColor2 = value;
        }

        public double GridTimestampThickness { get; set; } = 0.0;

        public float TextOpacity { get; set; } = 1f;

        public bool TextEnabled { get; set; } = false;

        public double TextStrokeThickness { get; set; } = 0.0;

        public string TextFormat { get; set; } = "{MachineName}:{UserName}";

        public string TextCustomDateTimeFormat { get; set; } = "dd/MM/yyyy";

        public int TextSize
        {
            get => this.textSize;
            set => this.textSize = value;
        }

        public int TextAngle
        {
            get => this.textAngle;
            set => this.textAngle = value;
        }

        public bool TextSpacingEnabled { get; set; } = true;

        public float TextBlurRadius { get; set; } = 0.0f;

        public bool TextAdjustment { get; set; } = false;

        public int TextSpacingX
        {
            get => this.textSpacingX;
            set => this.textSpacingX = value;
        }

        public int TextSpacingY
        {
            get => this.textSpacingY;
            set => this.textSpacingY = value;
        }

        public int TextRows { get; set; } = 4;

        public int TextCols { get; set; } = 5;

        public string TextColor1
        {
            get => this.textColor1;
            set => this.textColor1 = value;
        }

        public string TextColor2
        {
            get => this.textColor2;
            set => this.textColor2 = value;
        }

        public float LogoOpacity { get; set; } = 1f;

        public bool LogoEnabled { get; set; } = false;

        public double LogoTimestampThickness { get; set; } = 0.0;

        public string LogoURL { get; set; } = "https://assets.agilemark.io/images/agilemark-logo.png";

        public string LogoHash { get; set; } = "";

        public object LogoPosition { get; set; } = (object)new bool[3, 3];

        public bool LogoShowTimestamp { get; set; } = false;

        public bool CipherTextGridEnabled { get; set; } = false;

        public double CipherTextStrokeThickness { get; set; } = 0.0;

        public int CipherTextGridCols { get; set; } = 3;

        public int CipherTextGridRows { get; set; } = 3;

        public float CipherTextGridOpacity { get; set; } = 1f;

        public int CipherTextGridSize { get; set; } = 25;

        public int CipherTextGridAngle { get; set; } = 20;

        public string CipherTextGridColor1 { get; set; } = "#000000";

        public string CipherTextGridColor2 { get; set; } = "#333333";

        public float CipherTextGridBlurRadius { get; set; } = 10f;

        public char[,] CipherTextGridTemplate { get; set; } = (char[,])null;

        public int CipherTextGridSpacingX { get; set; } = 200;

        public int CipherTextGridSpacingY { get; set; } = 200;

        public bool CipherTextGridShowTimestamp { get; set; } = false;

        public bool CipherShapeGridEnabled { get; set; } = false;

        public double CipherShapeStrokeThickness { get; set; } = 0;

        public int CipherShapeGridCols { get; set; } = 3; // (***)

        public int CipherShapeGridRows { get; set; } = 3; // (***)

        public float CipherShapeGridOpacity { get; set; } = 1f;

        public int CipherShapeGridSize { get; set; } = 25;

        public int CipherShapeGridAngle { get; set; } = 20;

        public string CipherShapeGridColor1 { get; set; } = "#000000";

        public string CipherShapeGridColor2 { get; set; } = "#333333";

        public float CipherShapeGridBlurRadius { get; set; } = 10;

        public char[,] CipherShapeGridTemplate { get; set; } = null;

        public int CipherShapeGridSpacingX { get; set; } = 200;

        public int CipherShapeGridSpacingY { get; set; } = 200;

        public bool CipherShapeGridShowTimestamp { get; set; } = false;
        public void Validate()
        {
            this.Lock(ref this.opacity, -0.01f, 1f);
            this.Lock(ref this.cols, 3, 9);
            this.Lock(ref this.rows, 3, 9);
            this.Lock(ref this.cellWidth, 3f, 100f);
            this.Lock(ref this.cellHeight, 5f, 100f);
            this.Lock(ref this.gridSpacingX, 100, 2000);
            this.Lock(ref this.gridSpacingY, 100, 2000);
            this.Lock(ref this.randomOffsetDistance, 0, 2000);
            this.Lock(ref this.marginPercent, 0.0f, 0.25f);
            this.CheckColor(ref this.gridColor1);
            this.CheckColor(ref this.gridColor2);
            this.Lock(ref this.textSize, 14, 90);
            this.Lock(ref this.textAngle, 0, 360);
            this.Lock(ref this.textSpacingX, 100, 2000);
            this.Lock(ref this.textSpacingX, 100, 2000);
            this.CheckColor(ref this.textColor1);
            this.CheckColor(ref this.textColor2);
            int num1 = (int)Math.Ceiling((double)this.CellWidth) * 9;
            int num2 = (int)Math.Ceiling((double)this.CellHeight) * 5;
            if (this.GridSpacingX < num1)
                this.GridSpacingX = num1;
            if (this.GridSpacingY >= num2)
                return;
            this.GridSpacingY = num2;
        }

        public void Lock(ref float variable, float min, float max)
        {
            if ((double)variable < (double)min)
            {
                variable = min;
            }
            else
            {
                if ((double)variable <= (double)max)
                    return;
                variable = max;
            }
        }

        public void Lock(ref int variable, int min, int max)
        {
            if (variable < min)
            {
                variable = min;
            }
            else
            {
                if (variable <= max)
                    return;
                variable = max;
            }
        }

        public void CheckColor(ref string color)
        {
            if (Regex.Match(color, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
                return;
            color = "#095509";
        }
    }
}
