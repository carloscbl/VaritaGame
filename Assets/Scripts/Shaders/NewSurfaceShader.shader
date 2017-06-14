Shader "Myshaders/ChangeMaterial" {
	Properties{
		_TexMat1("Base (RGB)", 2D) = "white" {}
	_TexMat2("Base (RGB)", 2D) = "white" {}
	_TexMat3("Base (RGB)", 2D) = "white" {}
	_Blend("Blend", Range(0.0,1.0)) = 0.0
		_Color("Main Color", Color) = (1, 1, 1, 1)
	}

		Category{
		ZWrite On
		Alphatest Greater 0
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		SubShader{
		Pass{

		Material{
		Diffuse[unity_AmbientSky]
		Ambient[unity_AmbientSky]
	}
		Lighting On

		SetTexture[_TexMat1]{ combine texture }
		SetTexture[_TexMat2]{ constantColor(0,0,0,[_Blend]) combine texture lerp(constant) previous }
		SetTexture[_TexMat2]{ combine previous + -primary, previous * primary }
		SetTexture[_TexMat2]{ combine texture }
		SetTexture[_TexMat3]{ constantColor(0,0,0,[_Blend]) combine texture lerp(constant) previous }
		SetTexture[_TexMat3]{ combine previous + -primary, previous * primary }


	}
	}
		FallBack " Diffuse", 1
	}
}