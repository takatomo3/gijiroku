// MonobitEngine.VoiceChat.MonobitMicrophone
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using a;
using A;
using MonobitEngine.VoiceChat;
using MonobitEngine.VoiceChat.Codec.Opus;
using mrs;
using Private;
using UnityEngine;

public sealed class MonobitMicrophone : u
{
	public delegate bool v(int, int);

	public delegate bool W(float[], int, int);

	public delegate void w();

	public delegate void X(bool, object[], byte[], int);

	public delegate bool x();

	public delegate void Y();

	[CompilerGenerated]
	private sealed class y
	{
		internal MonobitMicrophone Dm;

		internal bool A(MonobitMicrophone P_0)
		{
			return (Object)(object)P_0 == (Object)(object)Dm;
		}
	}

	private aC m_encoder;

	private static string m_deviceName;

	private static List<MonobitMicrophone> m_recordingMicrophones;

	private V m_audioClip;

	private ac<float> m_RingBuf;

	private ac<float> m_ConvertRingBuf;

	private int m_minFreq;

	private int m_maxFreq;

	private int m_captureSamplingRate;

	private int m_captureFrameSizeWithChannel;

	private int m_frameSize;

	private int m_oldOffsetPos;

	private uint m_indexOfSequence;

	private int m_detectCountMax;

	private int m_detectCount;

	private bool m_isVoiceDetected;

	public v onBeginEncode = (int c, int s) => true;

	public W onPreEncode = (float[] v, int c, int s) => true;

	public w onEndEncode = delegate
	{
	};

	public X onSendRPC = delegate
	{
	};

	public x onMicrophoneError = () => true;

	public Y onMicrophoneRestart = delegate
	{
	};

	private bool m_bStopCapture;

	private bool m_bIsCapture;

	private int m_prevMicrophoneDevicesLength;

	public ushort Version
	{
		get;
		set;
	}

	public bool IsLocalPlayer
	{
		get;
		set;
	}

	public bool IsCapture
	{
		get
		{
			if (!IsLocalPlayer)
			{
				return false;
			}
			if (!m_bIsCapture)
			{
				if (!m_bStopCapture)
				{
					m_bIsCapture = Microphone.IsRecording(m_deviceName);
					return m_bIsCapture;
				}
				return Microphone.IsRecording(m_deviceName);
			}
			return true;
		}
	}

	public OpusCodec OpusCodecProp
	{
		get;
		set;
	}

	public FrameSizeMs FrameSizeMsProp
	{
		get;
		set;
	}

	public bool VoiceActivityDetectorProp
	{
		get;
		set;
	}

	public int TalkingThreshold
	{
		get;
		set;
	}

	public bool DebugModeProp
	{
		get;
		set;
	}

	public Application Application
	{
		get;
		set;
	}

	public OpusSignal OpusSignal
	{
		get;
		set;
	}

	public EncodeMode EncodeMode
	{
		get;
		set;
	}

	public int Complexity
	{
		get;
		set;
	}

	public bool ShowVoiceDataBps
	{
		get;
		set;
	}

	public int VoiceDataBps
	{
		get;
		set;
	}

	public static string MicrophoneDeviceName
	{
		get
		{
			return m_deviceName;
		}
		set
		{
			if (m_recordingMicrophones != null)
			{
				MonobitMicrophone[] array = m_recordingMicrophones.ToArray();
				foreach (MonobitMicrophone monobitMicrophone in array)
				{
					monobitMicrophone.ChangeInputDeviceName(value);
				}
			}
			m_deviceName = value;
		}
	}

	private static void AddRecordingMicrophone(MonobitMicrophone microphone)
	{
		if (m_recordingMicrophones == null)
		{
			m_recordingMicrophones = new List<MonobitMicrophone>();
		}
		m_recordingMicrophones.Add(microphone);
	}

	private static void RemoveRecordingMicrophone(MonobitMicrophone microphone)
	{
		if (m_recordingMicrophones != null)
		{
			m_recordingMicrophones.RemoveAll((MonobitMicrophone P_0) => (Object)(object)P_0 == (Object)(object)microphone);
		}
	}

	public void ChangeInputDeviceName(string name)
	{
		StopCapture();
		m_deviceName = name;
		StartCapture();
	}

	public bool StartCapture()
	{
		//Discarded unreachable code: IL_025f
		if (!isDeviceExist())
		{
			return false;
		}
		A(OpusCodecProp.Channels);
		a(OpusCodecProp.SamplingRateInfo.encodeSamplingRate);
		B(OpusCodecProp.BitsPerSample);
		b(OpusCodecProp.CompressedBitRate);
		m_captureSamplingRate = a();
		Application = OpusCodecProp.Application;
		OpusSignal = OpusCodecProp.OpusSignal;
		EncodeMode = OpusCodecProp.EncodeMode;
		Complexity = OpusCodecProp.Complexity;
		m_bIsCapture = false;
		m_bStopCapture = false;
		int bandWidth = (int)OpusCodecProp.BandWidth;
		Microphone.GetDeviceCaps(m_deviceName, ref m_minFreq, ref m_maxFreq);
		if (m_minFreq != 0 || m_maxFreq != 0)
		{
			m_captureSamplingRate = Mathf.Clamp(m_captureSamplingRate, m_minFreq, m_maxFreq);
		}
		m_encoder = new aC();
		if (m_encoder == null)
		{
			Debug.LogError((object)Private.PrivateClass.a());
			return false;
		}
		if (!m_encoder.A(A(), a(), B(), b(), bandWidth, (int)FrameSizeMsProp, m_captureSamplingRate, Application, OpusSignal, EncodeMode, Complexity))
		{
			Debug.LogError((object)Private.PrivateClass.B());
			return false;
		}
		int num = m_encoder.F();
		m_RingBuf = new ac<float>(num, 10);
		if (m_RingBuf == null)
		{
			return false;
		}
		if (a() != m_captureSamplingRate)
		{
			int num2 = m_encoder.D();
			m_ConvertRingBuf = new ac<float>(num2, 10);
		}
		m_frameSize = m_encoder.e();
		m_oldOffsetPos = 0;
		m_audioClip = new V();
		if (m_audioClip == null)
		{
			Debug.LogError((object)Private.PrivateClass.b());
			return false;
		}
		try
		{
			m_audioClip.A(Microphone.Start(m_deviceName, true, 5, m_captureSamplingRate));
			if ((Object)(object)m_audioClip.A() == (Object)null)
			{
				return false;
			}
		}
		catch (ArgumentException)
		{
			return false;
		}
		if (A() != m_audioClip.a())
		{
			Debug.Log((object)Private.PrivateClass.C());
			Microphone.End(m_deviceName);
			A(m_audioClip.a());
			m_encoder.a();
			m_RingBuf.Dispose();
			if (!m_encoder.A(A(), a(), B(), b(), bandWidth, (int)FrameSizeMsProp, m_captureSamplingRate, Application, OpusSignal, EncodeMode, Complexity))
			{
				Debug.LogError((object)Private.PrivateClass.B());
				return false;
			}
			num = m_encoder.F();
			m_RingBuf = new ac<float>(num, 10);
			if (m_RingBuf == null)
			{
				return false;
			}
			if (a() != m_captureSamplingRate)
			{
				int num3 = m_encoder.D();
				m_ConvertRingBuf = new ac<float>(num3, 10);
			}
			m_frameSize = m_encoder.e();
			m_audioClip.A(Microphone.Start(m_deviceName, true, 5, m_captureSamplingRate));
			if ((Object)(object)m_audioClip.A() == (Object)null)
			{
				Debug.LogError((object)Private.PrivateClass.c());
				return false;
			}
			AddRecordingMicrophone(this);
			return true;
		}
		Debug.Log((object)Private.PrivateClass.D());
		AddRecordingMicrophone(this);
		return true;
	}

	public void StopCapture()
	{
		RemoveRecordingMicrophone(this);
		if (IsCapture)
		{
			Microphone.End(m_deviceName);
		}
		if (m_audioClip != null)
		{
			m_audioClip.Dispose();
		}
		if (m_RingBuf != null)
		{
			m_RingBuf.Dispose();
		}
		if (m_encoder != null)
		{
			m_encoder.a();
		}
		m_RingBuf = null;
		m_audioClip = null;
		m_encoder = null;
		m_bIsCapture = false;
		m_bStopCapture = true;
		Debug.Log((object)Private.PrivateClass.d());
	}

	private bool isDeviceExist()
	{
		return Microphone.get_devices().Length > 0;
	}

	private bool DoProcessed(float[] voice)
	{
		if (voice.Length == 0 || m_encoder == null || m_audioClip == null)
		{
			Debug.LogError((object)Private.PrivateClass.E());
			return false;
		}
		if (!onBeginEncode(A(), a()))
		{
			return false;
		}
		if (a() != m_captureSamplingRate)
		{
			float[] array = m_ConvertRingBuf.A();
			float[] array2 = aD.A(voice, m_captureSamplingRate, array, a());
			if (array2 != null)
			{
				voice = array2;
			}
		}
		if (!onPreEncode(voice, A(), a()))
		{
			return false;
		}
		int num = 0;
		byte[] array3 = m_encoder.A(voice, out num);
		if (array3 == null || num == 0)
		{
			return false;
		}
		if (ShowVoiceDataBps)
		{
			VoiceDataBps += num * 8;
		}
		onEndEncode();
		byte[] buffer = BufferManager.New(BufferManager.GetAllocSize((uint)num));
		System.Buffer.BlockCopy(array3, 0, buffer, 0, num);
		m_indexOfSequence++;
		object[] array4 = new object[4]
		{
			Version,
			m_indexOfSequence,
			a(),
			(byte)((uint)A() & 0xFFu)
		};
		onSendRPC(DebugModeProp, array4, buffer, num);
		BufferManager.Delete(ref buffer);
		return true;
	}

	public void OnChangeBandWidth(OpusBandwidth bw)
	{
	}

	public void Awake()
	{
	}

	public void Start()
	{
		m_prevMicrophoneDevicesLength = Microphone.get_devices().Length;
	}

	public void Update()
	{
		int num = Microphone.get_devices().Length;
		if (num == 0 && IsCapture)
		{
			if (!m_bStopCapture)
			{
				StopCapture();
			}
			m_prevMicrophoneDevicesLength = 0;
			return;
		}
		if (0 < num && m_prevMicrophoneDevicesLength != num)
		{
			onMicrophoneRestart();
		}
		m_prevMicrophoneDevicesLength = num;
		if (!IsCapture || m_encoder == null || m_audioClip == null)
		{
			return;
		}
		if (m_RingBuf == null)
		{
			int num2 = m_encoder.F();
			m_RingBuf = new ac<float>(num2, 10);
			if (m_RingBuf == null)
			{
				return;
			}
		}
		int position = Microphone.GetPosition(m_deviceName);
		if (position >= m_oldOffsetPos + m_frameSize)
		{
			while (position >= m_oldOffsetPos + m_frameSize)
			{
				float[] array = m_RingBuf.A();
				if (m_audioClip.A(array, m_oldOffsetPos))
				{
					if (VoiceActivityDetectorProp)
					{
						if (VAD(array))
						{
							DoProcessed(array);
						}
					}
					else
					{
						DoProcessed(array);
					}
					m_oldOffsetPos += m_frameSize;
					continue;
				}
				if (onMicrophoneError())
				{
					StopCapture();
				}
				break;
			}
		}
		else
		{
			if (m_oldOffsetPos <= position)
			{
				return;
			}
			int num3 = m_audioClip.B();
			int num4 = num3 - m_oldOffsetPos + position;
			while (num4 >= m_frameSize)
			{
				float[] array2 = m_RingBuf.A();
				if (m_audioClip.A(array2, m_oldOffsetPos))
				{
					int num5 = m_oldOffsetPos + m_frameSize;
					if (num5 >= num3)
					{
						num5 -= num3;
					}
					int num6 = position - num5;
					if (VoiceActivityDetectorProp)
					{
						if (VAD(array2))
						{
							DoProcessed(array2);
						}
					}
					else
					{
						DoProcessed(array2);
					}
					m_oldOffsetPos = num5;
					num4 = num6;
				}
				else
				{
					m_oldOffsetPos += m_frameSize;
					if (m_oldOffsetPos >= num3)
					{
						m_oldOffsetPos -= num3;
					}
					num4 = position - m_oldOffsetPos;
				}
			}
		}
	}

	public void OnDestroy()
	{
		RemoveRecordingMicrophone(this);
	}

	private bool VAD(float[] voice)
	{
		float num = 0f;
		for (int i = 0; i < voice.Length; i++)
		{
			num += voice[i] * voice[i];
		}
		float num2 = Mathf.Sqrt(num / (float)voice.Length);
		float num3 = 20f * Mathf.Log10(num2 / 0.01f);
		if (num3 > (float)TalkingThreshold)
		{
			m_detectCount = 0;
			m_isVoiceDetected = true;
		}
		else
		{
			m_detectCount++;
		}
		if (m_detectCount > m_detectCountMax)
		{
			m_isVoiceDetected = false;
		}
		return m_isVoiceDetected;
	}

	public void SetVADLatitude(int latitude)
	{
		m_detectCountMax = latitude;
	}

	public AudioClip GetAudioClip()
	{
		if (m_audioClip == null)
		{
			return null;
		}
		return m_audioClip.A();
	}

	public int GetCaptureBufferSize()
	{
		return m_captureSamplingRate * 5;
	}

	public int GetCaptureSamplingRate()
	{
		return m_captureSamplingRate;
	}
}
