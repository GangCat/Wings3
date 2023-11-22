Shader "ShaderYJJ/ScreenShader"
{
    Properties
    {
        _powerRatio("_powerRatio", Float) = 0
        _Float("Float", Float) = 0
        _Frequency("Frequency", Float) = 100
        _Amplitude("Amplitude", Float) = 0.01
        [ToggleUI]_isDash("_isDash", Float) = 1
        [NoScaleOffset]_MainTex("_MainTex", 2D) = "white" {}
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            // RenderType: <None>
            "Queue" = "Transparent+1000"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalFullscreenSubTarget"
        }
        Pass
        {
            Name "DrawProcedural"
        
        // Render State
        Cull Off
        Blend Off
        ZTest Off
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma vertex vert
        #pragma fragment frag
        // #pragma enable_d3d11_debug_symbols
        
        /* WARNING: $splice Could not find named fragment 'DotsInstancingOptions' */
        /* WARNING: $splice Could not find named fragment 'HybridV1InjectedBuiltinProperties' */
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        #define FULLSCREEN_SHADERGRAPH
        
        // Defines
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_VERTEXID
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        
        // Force depth texture because we need it for almost every nodes
        // TODO: dependency system that triggers this define from position or view direction usage
        #define REQUIRE_DEPTH_TEXTURE
        #define REQUIRE_NORMAL_TEXTURE
        
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DRAWPROCEDURAL
        #define REQUIRE_OPAQUE_TEXTURE
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenShaderPass.cs.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
             uint vertexID : VERTEXID_SEMANTIC;
        };
        struct SurfaceDescriptionInputs
        {
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _powerRatio;
        float _Float;
        float _Frequency;
        float _Amplitude;
        float _isDash;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float _FlipY;
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Comparison_Less_float(float A, float B, out float Out)
        {
            Out = A < B ? 1 : 0;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_SceneColor_float(float4 UV, out float3 Out)
        {
            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        // GraphVertex: <None>
        
        // Custom interpolators, pre surface
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_71b66fd193674d3887851f24a5154870_Out_0_Boolean = _isDash;
            UnityTexture2D _Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Rotate_046297674e344d55a5725f8235060f77_Out_3_Vector2;
            Unity_Rotate_Radians_float(IN.uv0.xy, float2 (0.5, 0.5), IN.TimeParameters.x, _Rotate_046297674e344d55a5725f8235060f77_Out_3_Vector2);
            float4 _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D.tex, _Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D.samplerstate, _Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D.GetTransformedUV(_Rotate_046297674e344d55a5725f8235060f77_Out_3_Vector2) );
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_R_4_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.r;
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_G_5_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.g;
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_B_6_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.b;
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_A_7_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.a;
            float _Comparison_5db78136cd9148bca9a09bda9ee1462d_Out_2_Boolean;
            Unity_Comparison_Less_float(_SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_A_7_Float, 0.1, _Comparison_5db78136cd9148bca9a09bda9ee1462d_Out_2_Boolean);
            float4 _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_R_1_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[0];
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_G_2_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[1];
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_B_3_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[2];
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_A_4_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[3];
            float _Property_da48b9607d924898974d82de439416bf_Out_0_Float = _Frequency;
            float _Multiply_91395af752b64d51b3057acbb60c4116_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_da48b9607d924898974d82de439416bf_Out_0_Float, _Multiply_91395af752b64d51b3057acbb60c4116_Out_2_Float);
            float _Sine_d64b3f5a3bea4a97bccf9a010a662233_Out_1_Float;
            Unity_Sine_float(_Multiply_91395af752b64d51b3057acbb60c4116_Out_2_Float, _Sine_d64b3f5a3bea4a97bccf9a010a662233_Out_1_Float);
            float _Property_31f71858981a49609652e778a972e077_Out_0_Float = _Amplitude;
            float _Multiply_0e7e8d2b367b4bd9a7bcd816486cadeb_Out_2_Float;
            Unity_Multiply_float_float(_Sine_d64b3f5a3bea4a97bccf9a010a662233_Out_1_Float, _Property_31f71858981a49609652e778a972e077_Out_0_Float, _Multiply_0e7e8d2b367b4bd9a7bcd816486cadeb_Out_2_Float);
            float _Add_8d2468d176e842d89d3409668c5b6b7e_Out_2_Float;
            Unity_Add_float(_Split_6474299da8f14e2d9c5d4db0d60388b3_G_2_Float, _Multiply_0e7e8d2b367b4bd9a7bcd816486cadeb_Out_2_Float, _Add_8d2468d176e842d89d3409668c5b6b7e_Out_2_Float);
            float4 _Vector4_cfc0438b5cf04274b865bb11632e1916_Out_0_Vector4 = float4(_Split_6474299da8f14e2d9c5d4db0d60388b3_R_1_Float, _Add_8d2468d176e842d89d3409668c5b6b7e_Out_2_Float, _Split_6474299da8f14e2d9c5d4db0d60388b3_B_3_Float, _Split_6474299da8f14e2d9c5d4db0d60388b3_A_4_Float);
            float4 _Branch_00bdaf742a034d269f365e42e4443611_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_5db78136cd9148bca9a09bda9ee1462d_Out_2_Boolean, _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4, _Vector4_cfc0438b5cf04274b865bb11632e1916_Out_0_Vector4, _Branch_00bdaf742a034d269f365e42e4443611_Out_3_Vector4);
            float3 _SceneColor_3362e81ccd4b423ea64bc9b98a17f307_Out_1_Vector3;
            Unity_SceneColor_float(_Branch_00bdaf742a034d269f365e42e4443611_Out_3_Vector4, _SceneColor_3362e81ccd4b423ea64bc9b98a17f307_Out_1_Vector3);
            float3 _SceneColor_9c7b5bba33e34719ae9c5910e70efe11_Out_1_Vector3;
            Unity_SceneColor_float(float4(IN.NDCPosition.xy, 0, 0), _SceneColor_9c7b5bba33e34719ae9c5910e70efe11_Out_1_Vector3);
            float3 _Branch_cd63eeb391aa4914bb7a95720f1912f3_Out_3_Vector3;
            Unity_Branch_float3(_Property_71b66fd193674d3887851f24a5154870_Out_0_Boolean, _SceneColor_3362e81ccd4b423ea64bc9b98a17f307_Out_1_Vector3, _SceneColor_9c7b5bba33e34719ae9c5910e70efe11_Out_1_Vector3, _Branch_cd63eeb391aa4914bb7a95720f1912f3_Out_3_Vector3);
            surface.BaseColor = _Branch_cd63eeb391aa4914bb7a95720f1912f3_Out_3_Vector3;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            float3 normalWS = SHADERGRAPH_SAMPLE_SCENE_NORMAL(input.texCoord0.xy);
            float4 tangentWS = float4(0, 1, 0, 0); // We can't access the tangent in screen space
        
        
        
        
            float3 viewDirWS = normalize(input.texCoord1.xyz);
            float linearDepth = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(input.texCoord0.xy), _ZBufferParams);
            float3 cameraForward = -UNITY_MATRIX_V[2].xyz;
            float camearDistance = linearDepth / dot(viewDirWS, cameraForward);
            float3 positionWS = viewDirWS * camearDistance + GetCameraPositionWS();
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            output.NDCPosition = input.texCoord0.xy;
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenCommon.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenDrawProcedural.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "Blit"
        
        // Render State
        Cull Off
        Blend Off
        ZTest Off
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma vertex vert
        #pragma fragment frag
        // #pragma enable_d3d11_debug_symbols
        
        /* WARNING: $splice Could not find named fragment 'DotsInstancingOptions' */
        /* WARNING: $splice Could not find named fragment 'HybridV1InjectedBuiltinProperties' */
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        #define FULLSCREEN_SHADERGRAPH
        
        // Defines
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_VERTEXID
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        
        // Force depth texture because we need it for almost every nodes
        // TODO: dependency system that triggers this define from position or view direction usage
        #define REQUIRE_DEPTH_TEXTURE
        #define REQUIRE_NORMAL_TEXTURE
        
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_BLIT
        #define REQUIRE_OPAQUE_TEXTURE
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenShaderPass.cs.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
             uint vertexID : VERTEXID_SEMANTIC;
             float3 positionOS : POSITION;
        };
        struct SurfaceDescriptionInputs
        {
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float _powerRatio;
        float _Float;
        float _Frequency;
        float _Amplitude;
        float _isDash;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        float _FlipY;
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Comparison_Less_float(float A, float B, out float Out)
        {
            Out = A < B ? 1 : 0;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_SceneColor_float(float4 UV, out float3 Out)
        {
            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
        }
        
        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        // GraphVertex: <None>
        
        // Custom interpolators, pre surface
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_71b66fd193674d3887851f24a5154870_Out_0_Boolean = _isDash;
            UnityTexture2D _Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Rotate_046297674e344d55a5725f8235060f77_Out_3_Vector2;
            Unity_Rotate_Radians_float(IN.uv0.xy, float2 (0.5, 0.5), IN.TimeParameters.x, _Rotate_046297674e344d55a5725f8235060f77_Out_3_Vector2);
            float4 _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D.tex, _Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D.samplerstate, _Property_a640d59fbb2049bd98bb8789853e8c40_Out_0_Texture2D.GetTransformedUV(_Rotate_046297674e344d55a5725f8235060f77_Out_3_Vector2) );
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_R_4_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.r;
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_G_5_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.g;
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_B_6_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.b;
            float _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_A_7_Float = _SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_RGBA_0_Vector4.a;
            float _Comparison_5db78136cd9148bca9a09bda9ee1462d_Out_2_Boolean;
            Unity_Comparison_Less_float(_SampleTexture2D_7dd4eb0f8dc74ef3a60ba7cbfcd21df4_A_7_Float, 0.1, _Comparison_5db78136cd9148bca9a09bda9ee1462d_Out_2_Boolean);
            float4 _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_R_1_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[0];
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_G_2_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[1];
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_B_3_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[2];
            float _Split_6474299da8f14e2d9c5d4db0d60388b3_A_4_Float = _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4[3];
            float _Property_da48b9607d924898974d82de439416bf_Out_0_Float = _Frequency;
            float _Multiply_91395af752b64d51b3057acbb60c4116_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_da48b9607d924898974d82de439416bf_Out_0_Float, _Multiply_91395af752b64d51b3057acbb60c4116_Out_2_Float);
            float _Sine_d64b3f5a3bea4a97bccf9a010a662233_Out_1_Float;
            Unity_Sine_float(_Multiply_91395af752b64d51b3057acbb60c4116_Out_2_Float, _Sine_d64b3f5a3bea4a97bccf9a010a662233_Out_1_Float);
            float _Property_31f71858981a49609652e778a972e077_Out_0_Float = _Amplitude;
            float _Multiply_0e7e8d2b367b4bd9a7bcd816486cadeb_Out_2_Float;
            Unity_Multiply_float_float(_Sine_d64b3f5a3bea4a97bccf9a010a662233_Out_1_Float, _Property_31f71858981a49609652e778a972e077_Out_0_Float, _Multiply_0e7e8d2b367b4bd9a7bcd816486cadeb_Out_2_Float);
            float _Add_8d2468d176e842d89d3409668c5b6b7e_Out_2_Float;
            Unity_Add_float(_Split_6474299da8f14e2d9c5d4db0d60388b3_G_2_Float, _Multiply_0e7e8d2b367b4bd9a7bcd816486cadeb_Out_2_Float, _Add_8d2468d176e842d89d3409668c5b6b7e_Out_2_Float);
            float4 _Vector4_cfc0438b5cf04274b865bb11632e1916_Out_0_Vector4 = float4(_Split_6474299da8f14e2d9c5d4db0d60388b3_R_1_Float, _Add_8d2468d176e842d89d3409668c5b6b7e_Out_2_Float, _Split_6474299da8f14e2d9c5d4db0d60388b3_B_3_Float, _Split_6474299da8f14e2d9c5d4db0d60388b3_A_4_Float);
            float4 _Branch_00bdaf742a034d269f365e42e4443611_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_5db78136cd9148bca9a09bda9ee1462d_Out_2_Boolean, _ScreenPosition_ce5647c737074b2bbb728a08f3c73099_Out_0_Vector4, _Vector4_cfc0438b5cf04274b865bb11632e1916_Out_0_Vector4, _Branch_00bdaf742a034d269f365e42e4443611_Out_3_Vector4);
            float3 _SceneColor_3362e81ccd4b423ea64bc9b98a17f307_Out_1_Vector3;
            Unity_SceneColor_float(_Branch_00bdaf742a034d269f365e42e4443611_Out_3_Vector4, _SceneColor_3362e81ccd4b423ea64bc9b98a17f307_Out_1_Vector3);
            float3 _SceneColor_9c7b5bba33e34719ae9c5910e70efe11_Out_1_Vector3;
            Unity_SceneColor_float(float4(IN.NDCPosition.xy, 0, 0), _SceneColor_9c7b5bba33e34719ae9c5910e70efe11_Out_1_Vector3);
            float3 _Branch_cd63eeb391aa4914bb7a95720f1912f3_Out_3_Vector3;
            Unity_Branch_float3(_Property_71b66fd193674d3887851f24a5154870_Out_0_Boolean, _SceneColor_3362e81ccd4b423ea64bc9b98a17f307_Out_1_Vector3, _SceneColor_9c7b5bba33e34719ae9c5910e70efe11_Out_1_Vector3, _Branch_cd63eeb391aa4914bb7a95720f1912f3_Out_3_Vector3);
            surface.BaseColor = _Branch_cd63eeb391aa4914bb7a95720f1912f3_Out_3_Vector3;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            float3 normalWS = SHADERGRAPH_SAMPLE_SCENE_NORMAL(input.texCoord0.xy);
            float4 tangentWS = float4(0, 1, 0, 0); // We can't access the tangent in screen space
        
        
        
        
            float3 viewDirWS = normalize(input.texCoord1.xyz);
            float linearDepth = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(input.texCoord0.xy), _ZBufferParams);
            float3 cameraForward = -UNITY_MATRIX_V[2].xyz;
            float camearDistance = linearDepth / dot(viewDirWS, cameraForward);
            float3 positionWS = viewDirWS * camearDistance + GetCameraPositionWS();
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            output.NDCPosition = input.texCoord0.xy;
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenCommon.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenBlit.hlsl"
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}