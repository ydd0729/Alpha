/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@martinTayx)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            22-Nov-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy
{
    /// <summary>
    /// This class communicates directly with the shader to draw the graphs. Performance here is very important
    /// to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    public class G_GraphShader
    {
        #region Variables

        public const int ArrayMaxSizeFull = 512;
        public const int ArrayMaxSizeLight = 128;

        public int ArrayMaxSize = 128;

        public float[] ShaderArrayValues;
        
        public Image Image = null;

        public float Average = 0;
        
        public float GoodThreshold = 0;
        public float CautionThreshold = 0;

        public Color GoodColor = Color.white;
        public Color CautionColor = Color.white;
        public Color CriticalColor = Color.white;

        private static readonly int AveragePropertyId = Shader.PropertyToID( "Average" );

        private static readonly int GoodThresholdPropertyId = Shader.PropertyToID( "_GoodThreshold" );
        private static readonly int CautionThresholdPropertyId = Shader.PropertyToID( "_CautionThreshold" );
        
        private static readonly int GoodColorPropertyId = Shader.PropertyToID( "_GoodColor" );
        private static readonly int CautionColorPropertyId = Shader.PropertyToID( "_CautionColor" );
        private static readonly int CriticalColorPropertyId = Shader.PropertyToID( "_CriticalColor" );
        
        private static readonly int GraphValues = Shader.PropertyToID( "GraphValues" );
        private static readonly int GraphValuesLength = Shader.PropertyToID( "GraphValues_Length" );

        #endregion

        #region Methods -> Public

        /// <summary>
        /// Updates the average parameter in the material.
        /// </summary>
        public void UpdateAverage()
        {
            Image.material.SetFloat( AveragePropertyId, Average );
        }

        /// <summary>
        /// Updates the thresholds in the material.
        /// </summary>
        public void UpdateThresholds()
        {
            Image.material.SetFloat( GoodThresholdPropertyId, GoodThreshold );
            Image.material.SetFloat( CautionThresholdPropertyId, CautionThreshold );
        }

        /// <summary>
        /// Updates the colors in the material.
        /// </summary>
        public void UpdateColors()
        {
            Image.material.SetColor( GoodColorPropertyId, GoodColor );
            Image.material.SetColor( CautionColorPropertyId, CautionColor );
            Image.material.SetColor( CriticalColorPropertyId, CriticalColor );
        }

        /// <summary>
        /// Updates the points in the graph with the set array of values.
        /// </summary>
        public void UpdatePoints()
        {
            UpdateArrayValuesLength();
            Image.material.SetFloatArray(GraphValues, ShaderArrayValues);
        }

        #endregion
        
        #region Methods -> Private
        
        /// <summary>
        /// Updates the GraphValuesLength parameter in the material with the current length of the
        /// ShaderArrayValues float[] array.
        /// </summary>
        private void UpdateArrayValuesLength()
        {
            Image.material.SetInt( GraphValuesLength, ShaderArrayValues.Length );
        }
        
        #endregion
    }
}