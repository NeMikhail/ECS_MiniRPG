using UnityEngine;
using UnityEngine.UIElements;

namespace MAEngine.UI.CustomElements
{
    public enum FillMode
    {
        Ring,
        Filled
    }

    public class CircularProgressBar : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CircularProgressBar, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription m_Progress = new UxmlFloatAttributeDescription
            {
                name = "progress",
                defaultValue = 0f
            };

            UxmlColorAttributeDescription m_FillColor = new UxmlColorAttributeDescription
            {
                name = "fill-color",
                defaultValue = new Color(0.2f, 0.8f, 0.2f, 1f)
            };

            UxmlColorAttributeDescription m_BackgroundColor = new UxmlColorAttributeDescription
            {
                name = "background-color",
                defaultValue = new Color(0.2f, 0.2f, 0.2f, 0.5f)
            };

            UxmlFloatAttributeDescription m_Thickness = new UxmlFloatAttributeDescription
            {
                name = "thickness",
                defaultValue = 10f
            };

            UxmlFloatAttributeDescription m_Radius = new UxmlFloatAttributeDescription
            {
                name = "radius",
                defaultValue = 50f
            };

            UxmlEnumAttributeDescription<FillMode> m_FillMode = new UxmlEnumAttributeDescription<FillMode>
            {
                name = "fill-mode",
                defaultValue = FillMode.Ring
            };

            UxmlIntAttributeDescription m_SectorCount = new UxmlIntAttributeDescription
            {
                name = "sector-count",
                defaultValue = 0
            };

            UxmlFloatAttributeDescription m_SectorGap = new UxmlFloatAttributeDescription
            {
                name = "sector-gap",
                defaultValue = 4f
            };

            UxmlBoolAttributeDescription m_DiscreteProgress = new UxmlBoolAttributeDescription
            {
                name = "discrete-progress",
                defaultValue = false
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var element = ve as CircularProgressBar;

                element.Radius = m_Radius.GetValueFromBag(bag, cc);
                element.Thickness = m_Thickness.GetValueFromBag(bag, cc);
                element.Mode = m_FillMode.GetValueFromBag(bag, cc);

                element.FillColor = m_FillColor.GetValueFromBag(bag, cc);
                element.BackgroundRingColor = m_BackgroundColor.GetValueFromBag(bag, cc);

                element.SectorCount = m_SectorCount.GetValueFromBag(bag, cc);
                element.SectorGap = m_SectorGap.GetValueFromBag(bag, cc);
                element.DiscreteProgress = m_DiscreteProgress.GetValueFromBag(bag, cc);

                element.Progress = m_Progress.GetValueFromBag(bag, cc);
            }
        }

        private float m_Progress;
        private Color m_FillColor;
        private Color m_BackgroundRingColor;
        private float m_Thickness;
        private float m_Radius;
        private FillMode m_FillMode;
        private int m_SectorCount;
        private float m_SectorGap;
        private bool m_DiscreteProgress;

        public float Progress
        {
            get => m_Progress;
            set
            {
                m_Progress = Mathf.Clamp01(value);
                MarkDirtyRepaint();
            }
        }

        public Color FillColor
        {
            get => m_FillColor;
            set
            {
                m_FillColor = value;
                MarkDirtyRepaint();
            }
        }

        public Color BackgroundRingColor
        {
            get => m_BackgroundRingColor;
            set
            {
                m_BackgroundRingColor = value;
                MarkDirtyRepaint();
            }
        }

        public float Thickness
        {
            get => m_Thickness;
            set
            {
                m_Thickness = Mathf.Max(1f, value);
                MarkDirtyRepaint();
            }
        }

        public float Radius
        {
            get => m_Radius;
            set
            {
                m_Radius = Mathf.Max(10f, value);
                style.width = m_Radius * 2;
                style.height = m_Radius * 2;
                MarkDirtyRepaint();
            }
        }

        public FillMode Mode
        {
            get => m_FillMode;
            set
            {
                m_FillMode = value;
                MarkDirtyRepaint();
            }
        }

        public int SectorCount
        {
            get => m_SectorCount;
            set
            {
                m_SectorCount = Mathf.Max(0, value);
                MarkDirtyRepaint();
            }
        }

        public float SectorGap
        {
            get => m_SectorGap;
            set
            {
                m_SectorGap = Mathf.Max(0f, value);
                MarkDirtyRepaint();
            }
        }

        public bool DiscreteProgress
        {
            get => m_DiscreteProgress;
            set
            {
                m_DiscreteProgress = value;
                MarkDirtyRepaint();
            }
        }

        public CircularProgressBar()
        {
            m_Progress = 0f;
            m_FillColor = new Color(0.2f, 0.8f, 0.2f, 1f);
            m_BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            m_Thickness = 10f;
            m_Radius = 50f;
            m_FillMode = FillMode.Ring;
            m_SectorCount = 0;
            m_SectorGap = 4f;
            m_DiscreteProgress = false;

            generateVisualContent += OnGenerateVisualContent;

            style.width = m_Radius * 2;
            style.height = m_Radius * 2;
        }
        

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            float diameter = m_Radius * 2;
            Vector2 center = new Vector2(diameter / 2, diameter / 2);

            var painter = mgc.painter2D;

            if (m_FillMode == FillMode.Ring)
            {
                DrawRingMode(painter, center);
            }
            else
            {
                DrawFilledMode(painter, center);
            }
        }

        private void DrawRingMode(Painter2D painter, Vector2 center)
        {
            painter.lineWidth = m_Thickness;
            float drawRadius = m_Radius - m_Thickness / 2;

            if (HasSectors)
            {
                painter.lineCap = LineCap.Butt;
                DrawRingModeSectors(painter, center, drawRadius);
            }
            else
            {
                painter.lineCap = LineCap.Round;
                DrawRingModeClassic(painter, center, drawRadius);
            }
        }

        private void DrawRingModeClassic(Painter2D painter, Vector2 center, float drawRadius)
        {
            painter.strokeColor = m_BackgroundRingColor;
            painter.BeginPath();
            painter.Arc(center, drawRadius, 0, 360f);
            painter.Stroke();

            if (m_Progress > 0f)
            {
                painter.strokeColor = m_FillColor;
                painter.BeginPath();

                float startAngle = -90f;
                float sweepAngle = m_Progress * 360f;

                painter.Arc(center, drawRadius, startAngle, startAngle + sweepAngle);
                painter.Stroke();
            }
        }

        private void DrawRingModeSectors(Painter2D painter, Vector2 center, float drawRadius)
        {
            GetProgressSectorInfo(out int filledSectors, out float lastSectorFill);

            for (int i = 0; i < m_SectorCount; i++)
            {
                float sectorStart = GetSectorStartAngle(i);
                float sectorEnd = GetSectorEndAngle(i);

                painter.BeginPath();
                painter.Arc(center, drawRadius, sectorStart, sectorEnd);

                if (i < filledSectors)
                {
                    painter.strokeColor = m_FillColor;
                }
                else if (i == filledSectors && lastSectorFill > 0f)
                {
                    painter.strokeColor = m_BackgroundRingColor;
                    painter.Stroke();

                    float partialEnd = Mathf.Lerp(sectorStart, sectorEnd, lastSectorFill);
                    painter.strokeColor = m_FillColor;
                    painter.BeginPath();
                    painter.Arc(center, drawRadius, sectorStart, partialEnd);
                }
                else
                {
                    painter.strokeColor = m_BackgroundRingColor;
                }

                painter.Stroke();
            }
        }

        private void DrawFilledMode(Painter2D painter, Vector2 center)
        {
            if (HasSectors)
            {
                DrawFilledModeSectors(painter, center);
            }
            else
            {
                DrawFilledModeClassic(painter, center);
            }
        }

        private void DrawFilledModeClassic(Painter2D painter, Vector2 center)
        {
            painter.fillColor = m_BackgroundRingColor;
            painter.BeginPath();
            painter.Arc(center, m_Radius, 0, 360f);
            painter.Fill();

            if (m_Progress > 0f)
            {
                painter.fillColor = m_FillColor;

                if (m_Progress >= 0.999f)
                {
                    painter.BeginPath();
                    painter.Arc(center, m_Radius, 0, 360f);
                    painter.Fill();
                }
                else
                {
                    painter.BeginPath();

                    painter.MoveTo(center);

                    float startAngle = -90f;
                    float sweepAngle = m_Progress * 360f;

                    painter.Arc(center, m_Radius, startAngle, startAngle + sweepAngle);

                    painter.ClosePath();
                    painter.Fill();
                }
            }
        }

        private void DrawFilledModeSectors(Painter2D painter, Vector2 center)
        {
            GetProgressSectorInfo(out int filledSectors, out float lastSectorFill);

            for (int i = 0; i < m_SectorCount; i++)
            {
                float sectorStart = GetSectorStartAngle(i);
                float sectorEnd = GetSectorEndAngle(i);

                painter.BeginPath();
                painter.MoveTo(center);
                painter.Arc(center, m_Radius, sectorStart, sectorEnd);
                painter.ClosePath();

                if (i < filledSectors)
                {
                    painter.fillColor = m_FillColor;
                }
                else if (i == filledSectors && lastSectorFill > 0f)
                {
                    painter.fillColor = m_BackgroundRingColor;
                    painter.Fill();

                    float partialEnd = Mathf.Lerp(sectorStart, sectorEnd, lastSectorFill);
                    painter.fillColor = m_FillColor;
                    painter.BeginPath();
                    painter.MoveTo(center);
                    painter.Arc(center, m_Radius, sectorStart, partialEnd);
                    painter.ClosePath();
                }
                else
                {
                    painter.fillColor = m_BackgroundRingColor;
                }

                painter.Fill();
            }
        }

        public void SetProgress(float value)
        {
            Progress = value;
        }

        public void SetProgressAnimated(float targetValue, float duration)
        {
            float startValue = m_Progress;
            float elapsed = 0f;

            schedule.Execute(() =>
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                Progress = Mathf.Lerp(startValue, targetValue, t);

                if (t >= 1f)
                {
                    return;
                }
            }).Until(() => elapsed >= duration);
        }

        private bool HasSectors => m_SectorCount > 1;

        private float GetSectorAngle()
        {
            if (!HasSectors) return 360f;
            return 360f / m_SectorCount;
        }

        private float GetSectorStartAngle(int sectorIndex)
        {
            float sectorAngle = GetSectorAngle();
            return -90f + sectorIndex * sectorAngle + m_SectorGap / 2f;
        }

        private float GetSectorEndAngle(int sectorIndex)
        {
            float sectorAngle = GetSectorAngle();
            return -90f + (sectorIndex + 1) * sectorAngle - m_SectorGap / 2f;
        }

        private void GetProgressSectorInfo(out int filledSectors, out float lastSectorFill)
        {
            if (!HasSectors)
            {
                filledSectors = 0;
                lastSectorFill = m_Progress;
                return;
            }

            if (m_DiscreteProgress)
            {
                filledSectors = Mathf.FloorToInt(m_Progress * m_SectorCount);
                lastSectorFill = 0f;
            }
            else
            {
                float progressInSectors = m_Progress * m_SectorCount;
                filledSectors = Mathf.FloorToInt(progressInSectors);
                lastSectorFill = progressInSectors - filledSectors;
            }

            filledSectors = Mathf.Clamp(filledSectors, 0, m_SectorCount);
        }
    }
}
