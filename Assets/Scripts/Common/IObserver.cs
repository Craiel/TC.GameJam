using UnityEngine;
using System.Collections;

public interface IObserver
{
	void OnNotify(Subject subject, object args);
}
