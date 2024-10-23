namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ModApi;
    using ModApi.State;
    using ModApi.Common;
    using ModApi.Mods;
    using ModApi.Scenes.Events;
    using UnityEngine;
    using BeautifyEffect;
    using Assets.Scripts;
    using Assets.Scripts.Scenes;
    using ModApi.Levels;
    using ModApi.Ui.Inspector;
    using Object = System.Object;
    using UnityEditor;


    /// <summary>
    /// A singleton object representing this mod that is instantiated and initialize when the mod is loaded.
    /// </summary>
    public class Mod : ModApi.Mods.GameMod
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="Mod"/> class from being created.
        /// </summary>


        private Mod() : base()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the mod object.
        /// </summary>
        /// <value>The singleton instance of the mod object.</value>
        public static Mod Instance { get; } = GetModInstance<Mod>();


        /// <summary>
        /// Called when the mod is initialized.
        /// </summary>
        protected override void OnModInitialized()
        {
            base.OnModInitialized();
            Game.Instance.SceneManager.SceneLoaded += SceneManagerOnSceneLoaded;
            Game.Instance.UserInterface.AddBuildInspectorPanelAction(ModApi.Ui.Inspector.InspectorIds.FlightView, OnBuildFlightViewInspectorPanel);
        }

        private void SceneManagerOnSceneLoaded(Object sender, EventArgs e)
        {
            if (Game.InFlightScene)
            {
                var cam = Game.Instance.FlightScene?.ViewManager.GameView.GameCamera.NearCamera;
                cam.gameObject.AddComponent<LensEffects>();
                if (cam == null) Debug.Log("No camera found");
            }
        }

        private void OnBuildFlightViewInspectorPanel(BuildInspectorPanelRequest request) //flight panel settings
        {
            var g = new GroupModel("Lens Effects");
            request.Model.AddGroup(g);

            var dirtIntensity = ModSettings.Instance.lensDirtIntensity;
            var aberrationStrength = ModSettings.Instance.aberrationStrength;
            var vignetteRadius = ModSettings.Instance.vignetteRadius;
            var vignetteFeather = ModSettings.Instance.vignetteFeather;
            var allViewModes = ModSettings.Instance.allViewModes;

            var dirtIntensityModel = new SliderModel(
                "Lens dirt intensity", () => dirtIntensity.Value, s => dirtIntensity.UpdateAndCommit(s), dirtIntensity.Min, dirtIntensity.Max, false);

            var aberrationStrengthModel = new SliderModel(
                "Chromatic aberration strength", () => aberrationStrength.Value, s => aberrationStrength.UpdateAndCommit(s), aberrationStrength.Min, aberrationStrength.Max, false);

            var vignetteRadiusModel = new SliderModel(
                "Vignette radius", () => vignetteRadius.Value, s => vignetteRadius.UpdateAndCommit(s), vignetteRadius.Min, vignetteRadius.Max, false);

            var vignetteFeatherModel = new SliderModel(
                "Vignette feather", () => vignetteFeather.Value, s => vignetteFeather.UpdateAndCommit(s), vignetteFeather.Min, vignetteFeather.Max, false);

            var allViewModesModel = new ToggleModel(
                "Use effects in all viewmodes", () => allViewModes.Value, s => allViewModes.UpdateAndCommit(s));

            g.Add(dirtIntensityModel);
            g.Add(aberrationStrengthModel);
            g.Add(vignetteRadiusModel);
            g.Add(vignetteFeatherModel);
            g.Add(allViewModesModel);
        }
    }
}
