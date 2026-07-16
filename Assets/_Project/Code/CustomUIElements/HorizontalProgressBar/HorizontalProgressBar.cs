using UnityEngine;
using UnityEngine.UIElements;

namespace MAEngine.UI.CustomElements
{
    [UxmlElement]
    public partial class HorizontalProgressBar : VisualElement
    {
        private float _progress;
        private float _lowValue;
        private float _highValue;
        private Color _fillColor;
        private Color _backgroundColor;
        private Texture2D _fillTexture;
        private Texture2D _backgroundTexture;
        private int _sectorCount;
        private float _sectorGap;
        private bool _discreteProgress;

        [UxmlAttribute]
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = Mathf.Clamp(value, _lowValue, _highValue);
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public float LowValue
        {
            get => _lowValue;
            set
            {
                _lowValue = value;
                _progress = Mathf.Clamp(_progress, _lowValue, _highValue);
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public float HighValue
        {
            get => _highValue;
            set
            {
                _highValue = value;
                _progress = Mathf.Clamp(_progress, _lowValue, _highValue);
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
        public Texture2D FillTexture
        {
            get => _fillTexture;
            set
            {
                _fillTexture = value;
                MarkDirtyRepaint();
            }
        }

        [UxmlAttribute]
        public Texture2D BackgroundTexture
        {
            get => _backgroundTexture;
            set
            {
                _backgroundTexture = value;
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

        public HorizontalProgressBar()
        {
            _progress = 0f;
            _lowValue = 0f;
            _highValue = 100f;
            _fillColor = new Color(0.2f, 0.8f, 0.2f, 1f);
            _backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            _fillTexture = null;
            _backgroundTexture = null;
            _sectorCount = 0;
            _sectorGap = 4f;
            _discreteProgress = false;

            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (HasSectors)
            {
                DrawSectors(mgc);
            }
            else
            {
                DrawClassic(mgc);
            }
        }

        private void DrawClassic(MeshGenerationContext mgc)
        {
            float width = contentRect.width;
            float height = contentRect.height;

            DrawRect(mgc, 0f, width, 0f, height, _backgroundColor, _backgroundTexture, 0f, 1f);

            float normalizedProgress = GetNormalizedProgress();

            if (normalizedProgress <= 0f)
            {
                return;
            }

            float fillWidth = width * normalizedProgress;
            float fillUvRight = normalizedProgress;

            DrawRect(mgc, 0f, fillWidth, 0f, height, _fillColor, _fillTexture, 0f, fillUvRight);
        }

        private void DrawSectors(MeshGenerationContext mgc)
        {
            float width = contentRect.width;
            float height = contentRect.height;

            GetProgressSectorInfo(out int filledSectors, out float lastSectorFill);

            float sectorWidth = GetSectorWidth(width);

            for (int i = 0; i < _sectorCount; i++)
            {
                float sectorX = GetSectorX(i, sectorWidth);
                float sectorRight = sectorX + sectorWidth - _sectorGap;

                float uvLeft = sectorX / width;
                float uvRight = sectorRight / width;

                if (i < filledSectors)
                {
                    DrawRect(mgc, sectorX, sectorRight, 0f, height, _fillColor, _fillTexture, uvLeft, uvRight);
                }
                else if (i == filledSectors && lastSectorFill > 0f)
                {
                    DrawRect(mgc, sectorX, sectorRight, 0f, height, _backgroundColor, _backgroundTexture, uvLeft, uvRight);

                    float partialRight = sectorX + (sectorRight - sectorX) * lastSectorFill;
                    float partialUvRight = partialRight / width;

                    DrawRect(mgc, sectorX, partialRight, 0f, height, _fillColor, _fillTexture, uvLeft, partialUvRight);
                }
                else
                {
                    DrawRect(mgc, sectorX, sectorRight, 0f, height, _backgroundColor, _backgroundTexture, uvLeft, uvRight);
                }
            }
        }

        private void DrawRect(MeshGenerationContext mgc, float xLeft, float xRight, float yTop, float yBottom,
            Color color, Texture2D texture, float uvLeft, float uvRight)
        {
            var mesh = mgc.Allocate(4, 6, texture);

            mesh.SetNextVertex(new Vertex { position = new Vector3(xLeft, yTop, Vertex.nearZ), tint = color, uv = new Vector2(uvLeft, 1f) });
            mesh.SetNextVertex(new Vertex { position = new Vector3(xRight, yTop, Vertex.nearZ), tint = color, uv = new Vector2(uvRight, 1f) });
            mesh.SetNextVertex(new Vertex { position = new Vector3(xRight, yBottom, Vertex.nearZ), tint = color, uv = new Vector2(uvRight, 0f) });
            mesh.SetNextVertex(new Vertex { position = new Vector3(xLeft, yBottom, Vertex.nearZ), tint = color, uv = new Vector2(uvLeft, 0f) });

            mesh.SetNextIndex(0);
            mesh.SetNextIndex(1);
            mesh.SetNextIndex(2);
            mesh.SetNextIndex(0);
            mesh.SetNextIndex(2);
            mesh.SetNextIndex(3);
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

        private float GetNormalizedProgress()
        {
            float range = _highValue - _lowValue;
            if (range <= 0f) return 0f;
            return Mathf.Clamp01((_progress - _lowValue) / range);
        }

        private float GetSectorWidth(float totalWidth)
        {
            if (!HasSectors) return totalWidth;
            return totalWidth / _sectorCount;
        }

        private float GetSectorX(int sectorIndex, float sectorWidth)
        {
            return sectorIndex * sectorWidth;
        }

        private void GetProgressSectorInfo(out int filledSectors, out float lastSectorFill)
        {
            float normalizedProgress = GetNormalizedProgress();

            if (_discreteProgress)
            {
                filledSectors = Mathf.FloorToInt(normalizedProgress * _sectorCount);
                lastSectorFill = 0f;
            }
            else
            {
                float progressInSectors = normalizedProgress * _sectorCount;
                filledSectors = Mathf.FloorToInt(progressInSectors);
                lastSectorFill = progressInSectors - filledSectors;
            }

            filledSectors = Mathf.Clamp(filledSectors, 0, _sectorCount);
        }
    }
}
