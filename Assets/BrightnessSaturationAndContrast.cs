using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessSaturationAndContrast : PostEffectsBase
{
    public Shader briSatConShader;
    public Camera camera;
    private Material briSatConMaterial;
    //private:私有成员,在类的内部才可以访问;protected:保护成员,该类内部和继承类中可以访问
    public Material material
    {//构造函数(?)
        get
        {//get函数调用基类函数
            briSatConMaterial = CheckShaderAndCreateMaterial(briSatConShader, briSatConMaterial);
            return briSatConMaterial;
        }
    }

    [Range(0.0f, 3.0f)]//[Range]属性限制变量范围
    public float brightness = 1.0f;

    [Range(0.0f, 3.0f)]
    public float saturation = 1.0f;

    [Range(0.0f, 3.0f)]
    public float contrast = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float red = 0.0f;

    [Range(-1.0f, 1.0f)]
    public float green = 0.0f;

    [Range(-1.0f, 1.0f)]
    public float blue = 0.0f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)//在场景渲染完成后被调用,用来对屏幕的图像进行后处理
    {//默认private
        if (material != null)//材质可用
        {
            material.SetFloat("_Brightness", brightness);
            material.SetFloat("_Saturation", saturation);
            material.SetFloat("_Contrast", contrast);//参数传递给材质

            material.SetFloat("_Red", red);
            material.SetFloat("_Green", green);
            material.SetFloat("_Blue", blue);

            Graphics.Blit(src, dest, material);//处理
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

}
