namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi.Common;
    using ModApi.Settings.Core;
    

    /// <summary>
    /// The settings for the mod.
    /// </summary>
    /// <seealso cref="ModApi.Settings.Core.SettingsCategory{Assets.Scripts.ModSettings}" />
    public class ModSettings : SettingsCategory<ModSettings>
    {
        /// <summary>
        /// The mod settings instance.
        /// </summary>
        private static ModSettings _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModSettings"/> class.
        /// </summary>
        public ModSettings() : base("LensEffects")
        {
        }

        /// <summary>
        /// Gets the mod settings instance.
        /// </summary>
        /// <value>
        /// The mod settings instance.
        /// </value>
        public static ModSettings Instance => _instance ?? (_instance = Game.Instance.Settings.ModSettings.GetCategory<ModSettings>());
        ///// <summary>
        ///// Gets the TestSetting1 value
        ///// </summary>
        public enum vignettes{
            [EnumOption("Ellipse", "Elliptical shape")]ellipse,
            [EnumOption("Engineering Cam", "Circular shape")]engineeringCam
        };
        ///// <value>
        ///// The TestSetting1 value.
        ///// </value>
        //public NumericSetting<float> TestSetting1 { get; private set; }
        public NumericSetting<float> lensDirtIntensity { get; private set;}
        public NumericSetting<float> vignetteRadius { get; private set;}
        public NumericSetting<float> vignetteFeather { get; private set;}

        public EnumSetting<vignettes> vignetteType { get; private set;}
        public NumericSetting<float> aberrationStrength { get; private set;}
        public BoolSetting noisy { get; private set;}
        public BoolSetting allViewModes { get; private set;}
       
        /// <summary>
        /// Initializes the settings in the category.
        /// </summary>
        protected override void InitializeSettings()
        {
            //this.TestSetting1 = this.CreateNumeric<float>("Test Setting 1", 1f, 10f, 1f)
            //    .SetDescription("A test setting that does nothing.")
            //    .SetDisplayFormatter(x => x.ToString("F1"))
            //    .SetDefault(2f);
            this.lensDirtIntensity = this.CreateNumeric<float>("Lens Dirt Intensity",0f,1f,0.01f)
               .SetDescription("Intensity of the dirty lens effect")
               .SetDefault(0.95f);

            this.vignetteRadius = this.CreateNumeric<float>("Vignette Radius",0f,2f, 0.025f)
                .SetDescription("Radius of the vignette. I recommend leaving this at default unless you want something very specific")
                .SetDefault(0.6f);

            this.vignetteFeather = this.CreateNumeric<float>("Vignette Feather",0.5f,2f, 0.025f)
                .SetDescription("Controls the falloff strengh for the vignette")
                .SetDefault(1.5f);

            this.vignetteType = this.CreateEnum<vignettes>("Vignette Mode")
                .SetDescription("Texture type for the vignette")
                .SetDefault(vignettes.ellipse);

            this.aberrationStrength = this.CreateNumeric<float>("Chromatic Aberration Strength", 0f, 1f, 0.1f)
                .SetDescription("Amount of chromatic aberration applied to the distorted image")
                .SetDefault(0.6f);

            this.noisy = this.CreateBool("Radiation Noise")
                .SetDescription("Image will become noisy in vacuum")
                .SetDefault(true);

            this.allViewModes = this.CreateBool("Use filters on all viewmodes")
                .SetDescription("By default, lens dirt, vignettes etc. only are visible when viewing a camera. Enable this setting if you want to use filters on all viewmodes.")
                .SetDefault(false);
        }
    }
}