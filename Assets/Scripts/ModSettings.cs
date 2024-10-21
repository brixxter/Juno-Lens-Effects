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
        public NumericSetting<float> vignetteIntensity { get; private set;}
        public EnumSetting<vignettes> vignetteType { get; private set;}
        //public NumericSetting<float> distortionStrength { get; private set;}
        public NumericSetting<float> aberrationStrength { get; private set;}
        public BoolSetting allViewModes { get; private set;}
        public NumericSetting<int> shadowCascades { get; private set; }
      /*  public BoolSetting useDOF { get; private set;}
        public NumericSetting<float> dofDistance { get; private set;}
        public NumericSetting<float> dofBlurIntensity { get; private set;}
        public NumericSetting<float> dofAperture { get; private set;}*/

       
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

            this.vignetteIntensity = this.CreateNumeric<float>("Vignette Intensity",0f,1f, 0.025f)
                .SetDescription("Intensity of the vignette")
                .SetDefault(0.8f);

            this.vignetteType = this.CreateEnum<vignettes>("Vignette Mode")
                .SetDescription("Texture type for the vignette")
                .SetDefault(vignettes.ellipse);

            /*this.distortionStrength = this.CreateNumeric<float>("Lens Distortion Strength",-0.5f,0.5f,0.1f)
                .SetDescription("Amount of lens distortion applied in the shader. Positive values create barrel distortion, negative values create pincushion distortion")
                .SetDefault(0.2f);*/

            this.aberrationStrength = this.CreateNumeric<float>("Chromatic Aberration Strength", 0f, 1f, 0.1f)
                .SetDescription("Amount of chromatic aberration applied to the distorted image")
                .SetDefault(0.6f);

            this.allViewModes = this.CreateBool("Use filters on all viewmodes")
                .SetDescription("By default, lens dirt, vignettes etc. only are visible when viewing a camera. Enable this setting if you want to use filters on all viewmodes.")
                .SetDefault(false);

            this.shadowCascades = this.CreateNumeric<int>("Shadow Cascades", 0, 4, 1)
                .SetDefault(4);
        }
    }
}