using UnityEngine;
using UnityEngine.UIElements;

namespace CellarGames.UILibrary
{

    // An element that displays a circular progress meter using VectorAPI
    // Requires Unity 2022.1 or higher
    // Based on "Create a radial progress indicator" (https://docs.unity3d.com/Manual/UIE-radial-progress.html), 
    // but incorporates VectorAPI instead of a mesh
    public class RadialProgress : VisualElement
    {
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            // exposing the following to UXML.
            UxmlFloatAttributeDescription m_ProgressAttribute = new UxmlFloatAttributeDescription()
            {
                name = "progress"
            };
            UxmlFloatAttributeDescription m_LineWidthAttribute = new UxmlFloatAttributeDescription()
            {
                name = "lineWidth"
            };

            UxmlColorAttributeDescription m_LineColorAttribute = new UxmlColorAttributeDescription()
            {
                name = "lineColor"
            };

            UxmlColorAttributeDescription m_FillColorAttribute = new UxmlColorAttributeDescription()
            {
                name = "fillColor"
            };
  

            // The Init method is used to assign to the C# progress property from the value of the progress UXML
            // attribute.
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                (ve as RadialProgress).progress = m_ProgressAttribute.GetValueFromBag(bag, cc);
                (ve as RadialProgress).lineWidth = m_LineWidthAttribute.GetValueFromBag(bag, cc);
                (ve as RadialProgress).lineColor = m_LineColorAttribute.GetValueFromBag(bag, cc);
                (ve as RadialProgress).fillColor = m_FillColorAttribute.GetValueFromBag(bag, cc);
            }
        }

        // A Factory class is needed to expose this control to UXML.
        public new class UxmlFactory : UxmlFactory<RadialProgress, UxmlTraits> { }

        // These are USS class name for the control.
        public static readonly string ussClassName = "radial-progress";

        float m_Progress;
        float m_LineWidth;
        Color m_LineColor;
        Color m_FillColor;

        /// <summary>
        /// A value between 0 and 1
        /// </summary>
        public float progress
        {
            // The progress property is exposed in C#.
            get => m_Progress;
            set
            {
                // Whenever the progress property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                m_Progress = value;
                MarkDirtyRepaint();
            }
        }

        public float lineWidth
        {
            // The lineWidth property is exposed in C#.
            get => m_LineWidth;
            set
            {
                // Whenever the lineWidth property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                m_LineWidth = value;
                MarkDirtyRepaint();
            }
        }

        public Color lineColor
        {
            // The lineColor property is exposed in C#.
            get => m_LineColor;
            set
            {
                // Whenever the lineColor property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                m_LineColor = value;
                MarkDirtyRepaint();
            }
        }

        public Color fillColor
        {
            // The fillColor property is exposed in C#.
            get => m_FillColor;
            set
            {
                // Whenever the fillColor property changes, MarkDirtyRepaint() is named. This causes a call to the
                // generateVisualContents callback.
                m_FillColor = value;
                MarkDirtyRepaint();
            }
        }

        // This default constructor is RadialProgress's only constructor.
        public RadialProgress()
        {
            // Add the USS class name for the overall control.
            AddToClassList(ussClassName);

            // Register a callback to generate the visual content of the control.
            generateVisualContent += context => GenerateVisualContent(context);

            progress = 0.0f;
        }

        // Paint the partial circle using VectorAPI 
        static void GenerateVisualContent(MeshGenerationContext context)
        {
            RadialProgress element = (RadialProgress)context.visualElement;
            Painter2D paint2D = context.painter2D;

            // dimensions of progress bar are based on whichever is smaller of the width or height of the element
            float width = element.contentRect.width;
            float height = element.contentRect.height;
            Vector2 center = new Vector2(width / 2f, height / 2f);
            float radius = (Mathf.Min(width, height) - element.m_LineWidth) / 2f;

            float clampedProgress = Mathf.Clamp(element.m_Progress, 0f, 1f);

            // Draw the inner circle filled with fillColor. There is a gap between the inner disk and the outer disk equal to the line-width
            paint2D.fillColor = element.m_FillColor;
            paint2D.BeginPath();
            paint2D.Arc(center, radius - element.m_LineWidth, 0f, 360f);
            paint2D.Fill();

            // Draw the outer progress arc
            paint2D.strokeColor = element.m_LineColor;
            paint2D.lineWidth = element.m_LineWidth;
            paint2D.BeginPath();
            paint2D.Arc(center, radius, 0f, 360.0f * clampedProgress);
            paint2D.Stroke();
        }
    }
}