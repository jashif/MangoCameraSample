using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace CameraMangoSample.NewEffects
{
    public interface IEffect
    {
        // Methods
        WriteableBitmap Process(WriteableBitmap input);
        int[] Process(int[] inputPixels, int width, int height);

        // Properties
        string Name { get; }
    }
    public class EffectItem
    {
        // Fields

        // Methods
        public EffectItem(IEffect effect)
        {
            this.Effect = effect;
            this.Name = effect.Name;
        }

        public EffectItem(IEffect effect, string thumbnailRelativeResourcePath)
            : this(effect)
        {
            //this.Thumbnail = new WriteableBitmap(0, 0).FromResource(thumbnailRelativeResourcePath);
        }

        public EffectItem(IEffect effect, string thumbnailRelativeResourcePath, string name)
            : this(effect, thumbnailRelativeResourcePath)
        {
            this.Name = name;
        }

        // Properties
        public IEffect Effect
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ImageSource Thumbnail
        {
            get;
            set;
        }
    }
    public class NewEffects
    {

    }
    public class BitmapMixer
    {
        // Methods
        public BitmapMixer()
        {
            this.Mixture = 0.5f;
        }

        public WriteableBitmap Mix(WriteableBitmap input1, WriteableBitmap input2)
        {
            int width = input1.PixelWidth;
            int height = input1.PixelHeight;
            return this.Mix(input1.Pixels, input2.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Mix(int[] inputPixels1, int[] inputPixels2, int width, int height)
        {
            int[] numArray = new int[inputPixels1.Length];
            float mixture = this.Mixture;
            float num2 = 1f - mixture;
            for (int i = 0; i < inputPixels1.Length; i++)
            {
                int num4 = inputPixels1[i];
                byte num5 = (byte)(num4 >> 0x18);
                byte num6 = (byte)(num4 >> 0x10);
                byte num7 = (byte)(num4 >> 8);
                byte num8 = (byte)num4;
                int num9 = inputPixels2[i];
                byte num10 = (byte)(num9 >> 0x18);
                byte num11 = (byte)(num9 >> 0x10);
                byte num12 = (byte)(num9 >> 8);
                byte num13 = (byte)num9;
                numArray[i] = (((((byte)((num5 * num2) + (num10 * mixture))) << 0x18) | (((byte)((num6 * num2) + (num11 * mixture))) << 0x10)) | (((byte)((num7 * num2) + (num12 * mixture))) << 8)) | ((byte)((num8 * num2) + (num13 * mixture)));
            }
            return numArray;
        }

        // Properties
        public float Mixture
        {
            get;
            set;
        }
    }
    public class BrightnessContrastEffect : IEffect
    {


        // Methods
        public BrightnessContrastEffect()
        {
            this.BrightnessFactor = this.ContrastFactor = 0f;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = new int[inputPixels.Length];
            int num = (int)(this.BrightnessFactor * 255f);
            float num2 = (1f + this.ContrastFactor) / 1f;
            num2 *= num2;
            int num3 = (int)(num2 * 32768f);
            for (int i = 0; i < inputPixels.Length; i++)
            {
                int num5 = inputPixels[i];
                byte num6 = (byte)(num5 >> 0x18);
                byte num7 = (byte)(num5 >> 0x10);
                byte num8 = (byte)(num5 >> 8);
                byte num9 = (byte)num5;
                if (num != 0)
                {
                    int num10 = num7 + num;
                    int num11 = num8 + num;
                    int num12 = num9 + num;
                    num7 = (num10 > 0xff) ? ((byte)0xff) : ((num10 < 0) ? ((byte)0) : ((byte)num10));
                    num8 = (num11 > 0xff) ? ((byte)0xff) : ((num11 < 0) ? ((byte)0) : ((byte)num11));
                    num9 = (num12 > 0xff) ? ((byte)0xff) : ((num12 < 0) ? ((byte)0) : ((byte)num12));
                }
                if (num3 != 0)
                {
                    int num13 = num7 - 0x80;
                    int num14 = num8 - 0x80;
                    int num15 = num9 - 0x80;
                    num13 = (num13 * num3) >> 15;
                    num14 = (num14 * num3) >> 15;
                    num15 = (num15 * num3) >> 15;
                    num13 += 0x80;
                    num14 += 0x80;
                    num15 += 0x80;
                    num7 = (num13 > 0xff) ? ((byte)0xff) : ((num13 < 0) ? ((byte)0) : ((byte)num13));
                    num8 = (num14 > 0xff) ? ((byte)0xff) : ((num14 < 0) ? ((byte)0) : ((byte)num14));
                    num9 = (num15 > 0xff) ? ((byte)0xff) : ((num15 < 0) ? ((byte)0) : ((byte)num15));
                }
                numArray[i] = (((num6 << 0x18) | (num7 << 0x10)) | (num8 << 8)) | num9;
            }
            return numArray;
        }

        // Properties
        public float BrightnessFactor
        {

            get;
            set;
        }

        public float ContrastFactor
        {

            get;
            set;
        }

        public string Name
        {
            get
            {
                return "Brightness & Contrast";
            }
        }
    }
    public class SepiaEffect : IEffect
    {
        // Fields
        private readonly BrightnessContrastEffect contrastFx;
        private readonly TintEffect tintFx = TintEffect.Sepia;

        // Methods
        public SepiaEffect()
        {
            BrightnessContrastEffect effect = new BrightnessContrastEffect();
            effect.ContrastFactor = 0.05f;
            this.contrastFx = effect;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = this.tintFx.Process(inputPixels, width, height);
            return this.contrastFx.Process(numArray, width, height);
        }

        // Properties
        public float ContrastFactor
        {
            get
            {
                return this.contrastFx.ContrastFactor;
            }
            set
            {
                this.contrastFx.ContrastFactor = value;
            }
        }

        public string Name
        {
            get
            {
                return "Sepia";
            }
        }
    }
    public class TintEffect : IEffect
    {


        // Methods
        public TintEffect()
        {
            this.Color = Colors.White;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int width = (int)input.PixelWidth;
            int height = (int)input.PixelHeight;
            return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = new int[inputPixels.Length];
            byte num = this.Color.A;
            byte num2 = this.Color.R;
            byte num3 = this.Color.G;
            byte num4 = this.Color.B;
            for (int i = 0; i < inputPixels.Length; i++)
            {
                int num6 = inputPixels[i];
                byte num7 = (byte)(num6 >> 0x18);
                byte num8 = (byte)(num6 >> 0x10);
                byte num9 = (byte)(num6 >> 8);
                byte num10 = (byte)num6;
                int num11 = (((num8 * 0x1b36) + (num9 * 0x5b8c)) + (num10 * 0x93e)) >> 15;
                num7 = (byte)((num7 * num) >> 8);
                num8 = (byte)((num11 * num2) >> 8);
                num9 = (byte)((num11 * num3) >> 8);
                num10 = (byte)((num11 * num4) >> 8);
                numArray[i] = (((num7 << 0x18) | (num8 << 0x10)) | (num9 << 8)) | num10;
            }
            return numArray;
        }

        // Properties
        public Color Color
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return "Tint";
            }
        }

        public static TintEffect Sepia
        {
            get
            {
                TintEffect effect = new TintEffect();
                effect.Color = Color.FromArgb(0xff, 230, 0xb3, 0x4d);
                return effect;
            }
        }

        public static TintEffect White
        {
            get
            {
                TintEffect effect = new TintEffect();
                effect.Color = Colors.White;
                return effect;
            }
        }
    }
    public class VignetteEffect : IEffect
    {


        // Methods
        public VignetteEffect()
        {
            this.Size = 0.5f;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = new int[inputPixels.Length];
            int num = (width > height) ? ((height * 0x8000) / width) : ((width * 0x8000) / height);
            int num2 = width >> 1;
            int num3 = height >> 1;
            int num4 = (num2 * num2) + (num3 * num3);
            int num5 = (int)(num4 * (1f - this.Size));
            int num6 = num4 - num5;
            int index = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int num10 = inputPixels[index];
                    byte num11 = (byte)(num10 >> 0x18);
                    byte num12 = (byte)(num10 >> 0x10);
                    byte num13 = (byte)(num10 >> 8);
                    byte num14 = (byte)num10;
                    int num15 = num2 - j;
                    int num16 = num3 - i;
                    if (width > height)
                    {
                        num15 = (num15 * num) >> 15;
                    }
                    else
                    {
                        num16 = (num16 * num) >> 15;
                    }
                    int num17 = (num15 * num15) + (num16 * num16);
                    if (num17 > num5)
                    {
                        int num18 = ((num4 - num17) << 8) / num6;
                        num18 *= num18;
                        int num19 = (num12 * num18) >> 0x10;
                        int num20 = (num13 * num18) >> 0x10;
                        int num21 = (num14 * num18) >> 0x10;
                        num12 = (num19 > 0xff) ? ((byte)0xff) : ((num19 < 0) ? ((byte)0) : ((byte)num19));
                        num13 = (num20 > 0xff) ? ((byte)0xff) : ((num20 < 0) ? ((byte)0) : ((byte)num20));
                        num14 = (num21 > 0xff) ? ((byte)0xff) : ((num21 < 0) ? ((byte)0) : ((byte)num21));
                        num10 = (((num11 << 0x18) | (num12 << 0x10)) | (num13 << 8)) | num14;
                    }
                    numArray[index] = num10;
                    index++;
                }
            }
            return numArray;
        }

        // Properties
        public string Name
        {
            get
            {
                return "Vignette";
            }
        }

        public float Size
        {
            get;
            set;
        }
    }
    public class Watermarker
    {

        // Methods
        public Watermarker(string relativeResourcePath)
        {
            this.Watermark = new WriteableBitmap(0, 0).FromResource(relativeResourcePath);
            this.RelativeSize = 0.5f;
        }

        public WriteableBitmap Apply(WriteableBitmap input)
        {
            int width = this.Watermark.PixelWidth;
            int height = this.Watermark.PixelHeight;
            float num3 = ((float)width) / ((float)height);
            if (num3 > 1f)
            {
                width = (int)(input.PixelWidth * this.RelativeSize);
                height = (int)(((float)width) / num3);
            }
            else
            {
                height = (int)(input.PixelHeight * this.RelativeSize);
                width = (int)(height * num3);
            }
            WriteableBitmap source = this.Watermark.Resize(width, height, WriteableBitmapExtensions.Interpolation.Bilinear);
            WriteableBitmap bmp = input.Clone();
            Rect destRect = new Rect((double)(input.PixelWidth - width), (double)(input.PixelHeight - height), (double)width, (double)height);
            bmp.Blit(destRect, source, new Rect(0.0, 0.0, (double)width, (double)height));
            return bmp;
        }

        // Properties
        public float RelativeSize
        {
            get;
            set;
        }

        public WriteableBitmap Watermark
        {
            get;
            set;
        }
    }
    public class BlackWhiteEffect : IEffect
    {
        // Fields
        private readonly BrightnessContrastEffect contrastFx;
        private readonly TintEffect tintFx = TintEffect.White;

        // Methods
        public BlackWhiteEffect()
        {
            BrightnessContrastEffect effect = new BrightnessContrastEffect();
            effect.ContrastFactor = 0.15f;
            this.contrastFx = effect;
        }

        public WriteableBitmap Process(WriteableBitmap input)
        {
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
        }

        public int[] Process(int[] inputPixels, int width, int height)
        {
            int[] numArray = this.tintFx.Process(inputPixels, width, height);
            return this.contrastFx.Process(numArray, width, height);
        }

        // Properties
        public float ContrastFactor
        {
            get
            {
                return this.contrastFx.ContrastFactor;
            }
            set
            {
                this.contrastFx.ContrastFactor = value;
            }
        }

        public string Name
        {
            get
            {
                return "Black & White";
            }
        }
    }
    //    public class PolaroidEffect : IEffect
    //{
    //    // Fields
    //    private readonly GaussianBlurEffect blurFx;
    //    private readonly BitmapMixer mixer;
    //    private readonly TintEffect tintFx;
    //    private readonly VignetteEffect vignetteFx;

    //    // Methods
    //    public PolaroidEffect()
    //    {
    //        GaussianBlurEffect effect = new GaussianBlurEffect();
    //        effect.Sigma = 0.15f;
    //        this.blurFx = effect;
    //        this.vignetteFx = new VignetteEffect();
    //        this.tintFx = TintEffect.Sepia;
    //        BitmapMixer mixer = new BitmapMixer();
    //        mixer.Mixture = 0.5f;
    //        this.mixer = mixer;
    //    }

    //    public WriteableBitmap Process(WriteableBitmap input)
    //    {
    //        int width = input.PixelWidth;
    //        int height = input.PixelHeight;
    //        return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
    //    }

    //    public int[] Process(int[] inputPixels, int width, int height)
    //    {
    //        int[] numArray = this.blurFx.Process(inputPixels, width, height);
    //        numArray = this.vignetteFx.Process(numArray, width, height);
    //        int[] numArray2 = this.tintFx.Process(numArray, width, height);
    //        return this.mixer.Mix(numArray, numArray2, width, height);
    //    }

    //    // Properties
    //    public float Blurriness
    //    {
    //        get
    //        {
    //            return this.blurFx.Sigma;
    //        }
    //        set
    //        {
    //            this.blurFx.Sigma = value;
    //        }
    //    }

    //    public string Name
    //    {
    //        get
    //        {
    //            return "Poladroid";
    //        }
    //    }

    //    public Color TintColor
    //    {
    //        get
    //        {
    //            return this.tintFx.Color;
    //        }
    //        set
    //        {
    //            this.tintFx.Color = value;
    //        }
    //    }

    //    public float Tinting
    //    {
    //        get
    //        {
    //            return this.mixer.Mixture;
    //        }
    //        set
    //        {
    //            this.mixer.Mixture = value;
    //        }
    //    }

    //    public float Vignette
    //    {
    //        get
    //        {
    //            return this.vignetteFx.Size;
    //        }
    //        set
    //        {
    //            this.vignetteFx.Size = value;
    //        }
    //    }
    //}
    //    public class GaussianBlurEffect : IEffect
    //{

    //    private const int BytesPerPixel = 4;
    //    private const int Padding = 3;

    //    // Methods
    //    public GaussianBlurEffect()
    //    {
    //        this.Sigma = 1f;
    //    }

    //    private static float[] ApplyBlur(float[] srcPixels, int width, int height, float sigma)
    //    {
    //        float[] destinationArray = new float[srcPixels.Length];
    //        Array.Copy(srcPixels, destinationArray, destinationArray.Length);
    //        int num = width + 6;
    //        int num2 = height + 6;
    //        float num3 = sigma;
    //        float num4 = num3 * num3;
    //        float num5 = num4 * num3;
    //        float num6 = ((1.57825f + (2.44413f * num3)) + (1.4281f * num4)) + (0.422205f * num5);
    //        float num7 = ((2.44413f * num3) + (2.85619f * num4)) + (1.26661f * num5);
    //        float num8 = -((1.4281f * num4) + (1.26661f * num5));
    //        float num9 = 0.422205f * num5;
    //        float b = 1f - (((num7 + num8) + num9) / num6);
    //        ApplyPass(destinationArray, num, num2, num6, num7, num8, num9, b);
    //        float[] output = new float[destinationArray.Length];
    //        Transpose<float>(destinationArray, output, num, num2);
    //        ApplyPass(output, num2, num, num6, num7, num8, num9, b);
    //        Transpose<float>(output, destinationArray, num2, num);
    //        return destinationArray;
    //    }

    //    private static void ApplyPass(float[] pixels, int width, int height, float b0, float b1, float b2, float b3, float b)
    //    {
    //        float num = 1f / b0;
    //        int num2 = width * 4;
    //        for (int i = 0; i < height; i++)
    //        {
    //            int num4 = i * num2;
    //            for (int j = num4 + 12; j < (num4 + num2); j += 4)
    //            {
    //                FilterForward(j + 1, pixels, b, b1, b2, b3, num);
    //                FilterForward(j + 2, pixels, b, b1, b2, b3, num);
    //                FilterForward(j + 3, pixels, b, b1, b2, b3, num);
    //            }
    //            for (int k = ((num4 + num2) - 12) - 4; k >= num4; k -= 4)
    //            {
    //                FilterBackward(k + 1, pixels, b, b1, b2, b3, num);
    //                FilterBackward(k + 2, pixels, b, b1, b2, b3, num);
    //                FilterBackward(k + 3, pixels, b, b1, b2, b3, num);
    //            }
    //        }
    //    }

    //    private static void ArgbIntToFloat(int src, float[] tgt, int idx)
    //    {
    //        tgt[idx] = ((src >> 0x18) & 0xff) * 0.003921569f;
    //        tgt[idx + 1] = ((src >> 0x10) & 0xff) * 0.003921569f;
    //        tgt[idx + 2] = ((src >> 8) & 0xff) * 0.003921569f;
    //        tgt[idx + 3] = (src & 0xff) * 0.003921569f;
    //    }

    //    private static T[] ConvertImageWithPadding<TS, T>(TS[] source, int width, int height, int padding, TS paddedValue, Action<TS, T[], int> decompose)
    //    {
    //        int num = height + (2 * padding);
    //        int num2 = width + (2 * padding);
    //        T[] localArray = new T[(num * num2) * 4];
    //        for (int i = 0; i < padding; i++)
    //        {
    //            int num4 = num2 * i;
    //            for (int m = 0; m < padding; m++)
    //            {
    //                decompose.Invoke(paddedValue, localArray, (num4 + m) * 4);
    //            }
    //            for (int n = 0; n < width; n++)
    //            {
    //                decompose.Invoke(source[n], localArray, ((num4 + n) + padding) * 4);
    //            }
    //            for (int num7 = width; num7 < (width + padding); num7++)
    //            {
    //                decompose.Invoke(paddedValue, localArray, (num4 + num7) * 4);
    //            }
    //        }
    //        for (int j = 0; j < height; j++)
    //        {
    //            int num9 = num2 * (j + padding);
    //            int index = width * j;
    //            for (int num11 = 0; num11 < num2; num11++)
    //            {
    //                if (num11 < padding)
    //                {
    //                    decompose.Invoke(source[index], localArray, (num11 + num9) * 4);
    //                }
    //                else if (num11 >= (padding + width))
    //                {
    //                    decompose.Invoke(source[(index + width) - 1], localArray, (num11 + num9) * 4);
    //                }
    //                else
    //                {
    //                    decompose.Invoke(source[(index - padding) + num11], localArray, (num11 + num9) * 4);
    //                }
    //            }
    //        }
    //        for (int k = 0; k < padding; k++)
    //        {
    //            int num13 = num2 * ((height + padding) + k);
    //            int num14 = width * (height - 1);
    //            for (int num15 = 0; num15 < padding; num15++)
    //            {
    //                decompose.Invoke(paddedValue, localArray, (num13 + num15) * 4);
    //            }
    //            for (int num16 = 0; num16 < width; num16++)
    //            {
    //                decompose.Invoke(source[num14 + num16], localArray, ((num13 + num16) + padding) * 4);
    //            }
    //            for (int num17 = width; num17 < (width + padding); num17++)
    //            {
    //                decompose.Invoke(paddedValue, localArray, (num13 + num17) * 4);
    //            }
    //        }
    //        return localArray;
    //    }

    //    private static void FilterBackward(int i, float[] pixels, float b, float b1, float b2, float b3, float ib0)
    //    {
    //        pixels[i] = (b * pixels[i]) + ((((b1 * pixels[i + 4]) + (b2 * pixels[i + 8])) + (b3 * pixels[i + 12])) * ib0);
    //    }

    //    private static void FilterForward(int i, float[] pixels, float b, float b1, float b2, float b3, float ib0)
    //    {
    //        pixels[i] = (b * pixels[i]) + ((((b1 * pixels[i - 4]) + (b2 * pixels[i - 8])) + (b3 * pixels[i - 12])) * ib0);
    //    }

    //    public WriteableBitmap Process(WriteableBitmap input)
    //    {
    //        int width = input.PixelWidth;
    //        int height = input.PixelHeight;
    //        return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
    //    }

    //    public int[] Process(int[] inputPixels, int width, int height)
    //    {
    //        int[] numArray = new int[inputPixels.Length];
    //        float[] numArray3 = ApplyBlur(ConvertImageWithPadding<int, float>(inputPixels, width, height, 3, -16777216, new Action<int, float[], int>(null, (IntPtr) ArgbIntToFloat)), width, height, this.Sigma);
    //        for (int i = 0; i < height; i++)
    //        {
    //            int num2 = ((i + 3) * (width + 6)) + 3;
    //            int num3 = i * width;
    //            for (int j = 0; j < width; j++)
    //            {
    //                int num5 = (num2 + j) * 4;
    //                int num6 = inputPixels[num3 + j] >> 0x18;
    //                numArray[num3 + j] = (((((byte) num6) << 0x18) | (((byte) (numArray3[num5 + 1] * 255f)) << 0x10)) | (((byte) (numArray3[num5 + 2] * 255f)) << 8)) | ((byte) (numArray3[num5 + 3] * 255f));
    //            }
    //        }
    //        return numArray;
    //    }

    //    private static void Transpose<T>(T[] input, T[] output, int width, int height)
    //    {
    //        for (int i = 0; i < height; i++)
    //        {
    //            int num2 = (i * width) * 4;
    //            for (int j = 0; j < width; j++)
    //            {
    //                int num4 = (j * height) * 4;
    //                output[num4 + (i * 4)] = input[num2 + (j * 4)];
    //                output[(num4 + (i * 4)) + 1] = input[(num2 + (j * 4)) + 1];
    //                output[(num4 + (i * 4)) + 2] = input[(num2 + (j * 4)) + 2];
    //                output[(num4 + (i * 4)) + 3] = input[(num2 + (j * 4)) + 3];
    //            }
    //        }
    //    }

    //    // Properties
    //    public string Name
    //    {
    //        get
    //        {
    //            return "Blur";
    //        }
    //    }

    //    public float Sigma
    //    {
    //        get;set;
    //    }
    //}

    //   public class TiltShiftEffect : IEffect
    //{
    //    // Fields

    //    private readonly GaussianBlurEffect blurFx;
    //    private int[] blurredPixels;
    //    private int[] contrastedPixels;
    //    private readonly BrightnessContrastEffect contrastFx;
    //    private const float MaxFallOffFactor = 0.3f;

    //    // Methods
    //    public TiltShiftEffect()
    //    {
    //        this.UpperFallOff = 0.3f;
    //        this.LowerFallOff = 0.7f;
    //        GaussianBlurEffect effect = new GaussianBlurEffect();
    //        effect.Sigma = 1.25f;
    //        this.blurFx = effect;
    //        BrightnessContrastEffect effect2 = new BrightnessContrastEffect();
    //        effect2.ContrastFactor = 0.1f;
    //        this.contrastFx = effect2;
    //    }

    //    private void CreateBlurredBitmap(int[] inputPixels, int width, int height)
    //    {
    //        this.contrastedPixels = this.contrastFx.Process(inputPixels, width, height);
    //        this.blurredPixels = this.blurFx.Process(this.contrastedPixels, width, height);
    //    }

    //    public WriteableBitmap Process(WriteableBitmap input)
    //    {
    //        int width = input.PixelWidth;
    //        int height = input.PixelHeight;
    //        return this.Process(input.Pixels, width, height).ToWriteableBitmap(width, height);
    //    }

    //    public int[] Process(int[] inputPixels, int width, int height)
    //    {
    //        this.CreateBlurredBitmap(inputPixels, width, height);
    //        return this.ProcessOnlyFocusFadeOff(inputPixels, width, height);
    //    }

    //    public WriteableBitmap ProcessOnlyFocusFadeOff(WriteableBitmap input)
    //    {
    //        int width = input.PixelWidth;
    //        int height = input.PixelHeight;
    //        return this.ProcessOnlyFocusFadeOff(input.Pixels, width, height).ToWriteableBitmap(width, height);
    //    }

    //    public int[] ProcessOnlyFocusFadeOff(int[] inputPixels, int width, int height)
    //    {
    //        if ((this.contrastedPixels == null) || (this.blurredPixels == null))
    //        {
    //            this.CreateBlurredBitmap(inputPixels, width, height);
    //        }
    //        int[] blurredPixels = this.blurredPixels;
    //        if (this.UpperFallOff < this.LowerFallOff)
    //        {
    //            blurredPixels = new int[inputPixels.Length];
    //            int num = (int) (this.UpperFallOff * height);
    //            int num2 = (int) (this.LowerFallOff * height);
    //            int num3 = (num2 - num) >> 1;
    //            int num4 = num + num3;
    //            int num5 = num4;
    //            int num6 = num4;
    //            if (num3 > (height * 0.3f))
    //            {
    //                num3 = (int) (height * 0.3f);
    //                num5 = num + num3;
    //                num6 = num2 - num3;
    //            }
    //            float num7 = 1f / ((float) num3);
    //            int index = 0;
    //            for (int i = 0; i < height; i++)
    //            {
    //                for (int j = 0; j < width; j++)
    //                {
    //                    int num11 = this.contrastedPixels[index];
    //                    if ((i < num5) || (i > num6))
    //                    {
    //                        int num12 = this.blurredPixels[index];
    //                        if ((i > num) || (i < num2))
    //                        {
    //                            byte num13 = (byte) (num12 >> 0x18);
    //                            byte num14 = (byte) (num12 >> 0x10);
    //                            byte num15 = (byte) (num12 >> 8);
    //                            byte num16 = (byte) num12;
    //                            byte num17 = (byte) (num11 >> 0x18);
    //                            byte num18 = (byte) (num11 >> 0x10);
    //                            byte num19 = (byte) (num11 >> 8);
    //                            byte num20 = (byte) num11;
    //                            float num21 = (i < num4) ? ((float) (num5 - i)) : ((float) (i - num6));
    //                            num21 *= num7;
    //                            if (num21 > 1f)
    //                            {
    //                                num21 = 1f;
    //                            }
    //                            float num22 = 1f - num21;
    //                            num12 = (((((byte) ((num13 * num21) + (num17 * num22))) << 0x18) | (((byte) ((num14 * num21) + (num18 * num22))) << 0x10)) | (((byte) ((num15 * num21) + (num19 * num22))) << 8)) | ((byte) ((num16 * num21) + (num20 * num22)));
    //                        }
    //                        blurredPixels[index] = num12;
    //                    }
    //                    else
    //                    {
    //                        blurredPixels[index] = num11;
    //                    }
    //                    index++;
    //                }
    //            }
    //        }
    //        return blurredPixels;
    //    }

    //    // Properties
    //    public float Blurriness
    //    {
    //        get
    //        {
    //            return this.blurFx.Sigma;
    //        }
    //        set
    //        {
    //            this.blurFx.Sigma = value;
    //        }
    //    }

    //    public float ContrastFactor
    //    {
    //        get
    //        {
    //            return this.contrastFx.ContrastFactor;
    //        }
    //        set
    //        {
    //            this.contrastFx.ContrastFactor = value;
    //        }
    //    }

    //    public float LowerFallOff
    //    {
    //        get;set;
    //    }

    //    public string Name
    //    {
    //        get
    //        {
    //            return "Tilt Shift";
    //        }
    //    }

    //    public float UpperFallOff
    //    {
    //       get;set;
    //    }
    //}
}
namespace System
{
    public static class ArrayExtensions
    {
        // Methods
        public static WriteableBitmap ToWriteableBitmap(this int[] input, int width, int height)
        {
            WriteableBitmap bitmap = new WriteableBitmap(width, height);
            Buffer.BlockCopy(input, 0, bitmap.Pixels, 0, input.Length * 4);
            return bitmap;
        }
    }


}

 
