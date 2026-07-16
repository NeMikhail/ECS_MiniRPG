using UnityEngine;
using UnityEngine.UIElements;

namespace MAEngine.UI.CustomElements
{
    [UxmlElement]
    public partial class VerticalProgressBar : VisualElement
    {
        private float _progress;
        private Color _fillColor;
        private Color _backgroundColor;
        private float _barWidth;
        private float _barHeight;
        private int _sectorCount;
        private float _sectorGap;
        private bool _discreteProgress;

        [UxmlAttribute]
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = Mathf.Clamp01(value);
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public float BarWidth
        {
            get => _barWidth;
            set
            {
                _barWidth = Mathf.Max(1f, value);
                style.width = _barWidth;
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public float BarHeight
        {
            get => _barHeight;
            set
            {
                _barHeight = Mathf.Max(1f, value);
                style.height = _barHeight;
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public int SectorCount
        {
            get => _sectorCount;
            set
            {
                _sectorCount = Mathf.Max(0, value);
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public float SectorGap
        {
            get => _sectorGap;
            set
            {
                _sectorGap = Mathf.Max(0f, value);
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public bool DiscreteProgress
        {
            get => _discreteProgress;
            set
            {
                _discreteProgress = value;
                MarkDirtyRepaint();
            }
        }

        public VerticalProgressBar()
        {
            _progress = 0f;
            _fillColor = new Color(0.2f, 0.8f, 0.2f, 1f);
            _backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            _barWidth = 20f;
            _barHeight = 100f;
            _sectorCount = 0;
            _sectorGap = 4f;
            _discreteProgress = false;

            generateVisualContent += OnGenerateVisualContent;

            style.width = _barWidth;
            style.height = _barHeight;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var painter = mgc.painter2D;

            if (HasSectors)
            {
                DrawSectors(painter);
            }
            else
            {
                DrawClassic(painter);
            }
        }

        private void DrawClassic(Painter2D painter)
        {
            painter.fillColor = _backgroundColor;
            painter.BeginPath();
            painter.MoveTo(new Vector2(0, 0));
            painter.LineTo(new Vector2(_barWidth, 0));
            painter.LineTo(new Vector2(_barWidth, _barHeight));
            painter.LineTo(new Vector2(0, _barHeight));
            painter.ClosePath();
            painter.Fill();

            if (_progress <= 0f)
            {
                return;
            }

            float fillHeight = _barHeight * _progress;
            float fillY = _barHeight - fillHeight;

            painter.fillColor = _fillColor;
            painter.BeginPath();
            painter.MoveTo(new Vector2(0, fillY));
            painter.LineTo(new Vector2(_barWidth, fillY));
            painter.LineTo(new Vector2(_barWidth, _barHeight));
            painter.LineTo(new Vector2(0, _barHeight));
            painter.ClosePath();
            painter.Fill();
        }

        private void DrawSectors(Painter2D painter)
        {
            GetProgressSectorInfo(out int filledSectors, out float lastSectorFill);

            float sectorHeight = GetSectorHeight();

            for (int i = 0; i < _sectorCount; i++)
            {
                float sectorY = GetSectorY(i, sectorHeight);
                float sectorBottom = sectorY + sectorHeight - _sectorGap;

                painter.BeginPath();
                painter.MoveTo(new Vector2(0, sectorY));
                painter.LineTo(new Vector2(_barWidth, sectorY));
                painter.LineTo(new Vector2(_barWidth, sectorBottom));
                painter.LineTo(new Vector2(0, sectorBottom));
                painter.ClosePath();

                int reversedIndex = _sectorCount - 1 - i;

                if (reversedIndex < filledSectors)
                {
                    painter.fillColor = _fillColor;
                    painter.Fill();
                }
                else if (reversedIndex == filledSectors && lastSectorFill > 0f)
                {
                    painter.fillColor = _backgroundColor;
                    painter.Fill();

                    float partialFillHeight = (sectorBottom - sectorY) * lastSectorFill;
                    float partialTop = sectorBottom - partialFillHeight;

                    painter.fillColor = _fillColor;
                    painter.BeginPath();
                    painter.MoveTo(new Vector2(0, partialTop));
                    painter.LineTo(new Vector2(_barWidth, partialTop));
                    painter.LineTo(new Vector2(_barWidth, sectorBottom));
                    painter.LineTo(new Vector2(0, sectorBottom));
                    painter.ClosePath();
                    painter.Fill();
                }
                else
                {
                    painter.fillColor = _backgroundColor;
                    painter.Fill();
                }
            }
        }

        public void SetProgress(float value)
        {
            Progress = value;
        }

        public void SetProgressAnimated(float targetValue, float duration)
        {
            float startValue = _progress;
            float elapsed = 0f;

            schedule.Execute(() =>
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                Progress = Mathf.Lerp(startValue, targetValue, t);
            }).Until(() => elapsed >= duration);
        }

        private bool HasSectors => _sectorCount > 1;

        private float GetSectorHeight()
        {
            if (!HasSectors) return _barHeight;
            return _barHeight / _sectorCount;
        }

        private float GetSectorY(int sectorIndex, float sectorHeight)
        {
            return sectorIndex * sectorHeight;
        }

        private void GetProgressSectorInfo(out int filledSectors, out float lastSectorFill)
        {
            if (!HasSectors)
            {
                filledSectors = 0;
                lastSectorFill = _progress;
                return;
            }

            if (_discreteProgress)
            {
                filledSectors = Mathf.FloorToInt(_progress * _sectorCount);
                lastSectorFill = 0f;
            }
            else
            {
                float progressInSectors = _progress * _sectorCount;
                filledSectors = Mathf.FloorToInt(progressInSectors);
                lastSectorFill = progressInSectors - filledSectors;
            }

            filledSectors = Mathf.Clamp(filledSectors, 0, _sectorCount);
        }
    }
}
