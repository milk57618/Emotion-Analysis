package md525ecff11cf9568dc168267811841dc5c;


public class FaceTask
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"n_onPreExecute:()V:GetOnPreExecuteHandler\n" +
			"n_onProgressUpdate:([Ljava/lang/Object;)V:GetOnProgressUpdate_arrayLjava_lang_Object_Handler\n" +
			"n_onPostExecute:(Ljava/lang/Object;)V:GetOnPostExecute_Ljava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("RCamera.FaceTask, RCamera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FaceTask.class, __md_methods);
	}


	public FaceTask () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FaceTask.class)
			mono.android.TypeManager.Activate ("RCamera.FaceTask, RCamera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public FaceTask (md525ecff11cf9568dc168267811841dc5c.MainActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == FaceTask.class)
			mono.android.TypeManager.Activate ("RCamera.FaceTask, RCamera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "RCamera.MainActivity, RCamera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public java.lang.Object doInBackground (java.lang.Object[] p0)
	{
		return n_doInBackground (p0);
	}

	private native java.lang.Object n_doInBackground (java.lang.Object[] p0);


	public void onPreExecute ()
	{
		n_onPreExecute ();
	}

	private native void n_onPreExecute ();


	public void onProgressUpdate (java.lang.Object[] p0)
	{
		n_onProgressUpdate (p0);
	}

	private native void n_onProgressUpdate (java.lang.Object[] p0);


	public void onPostExecute (java.lang.Object p0)
	{
		n_onPostExecute (p0);
	}

	private native void n_onPostExecute (java.lang.Object p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
