// MountainPlacer.cs - Editor Klasörünün Ýçine Yerleþtirin
using UnityEngine;
using UnityEditor; // Editör araçlarý için bu zorunludur!

public class MountainPlacer : EditorWindow
{
    // Yerleþtirilecek Dað Prefab'ý
    private GameObject mountainPrefab;
    // Daðlar Arasýndaki Mesafe
    private float spacing = 10f;
    // Yerleþtirmenin Baþlangýç Noktasý
    private Transform startPoint;
    // Yerleþtirmenin Bitiþ Noktasý
    private Transform endPoint;

    // Pencereyi Unity menüsüne ekler
    [MenuItem("Window/Custom Tools/Mountain Placer")]
    public static void ShowWindow()
    {
        GetWindow<MountainPlacer>("Mountain Placer");
    }

    // Pencere Ýçindeki Arayüz
    void OnGUI()
    {
        GUILayout.Label("Mountain Placer Settings", EditorStyles.boldLabel);

        // Prefab Alaný
        mountainPrefab = (GameObject)EditorGUILayout.ObjectField("Mountain Prefab", mountainPrefab, typeof(GameObject), false);

        // Mesafe Alaný
        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        // Baþlangýç ve Bitiþ Noktasý Alanlarý (Transform referanslarý)
        startPoint = (Transform)EditorGUILayout.ObjectField("Start Point (Transform)", startPoint, typeof(Transform), true);
        endPoint = (Transform)EditorGUILayout.ObjectField("End Point (Transform)", endPoint, typeof(Transform), true);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Place Mountains"))
        {
            PlaceMountains();
        }
    }

    // Daðlarý Yerleþtirme Fonksiyonu
    void PlaceMountains()
    {
        if (mountainPrefab == null || startPoint == null || endPoint == null)
        {
            Debug.LogError("Lütfen Prefab, Baþlangýç ve Bitiþ Noktalarýný atayýn.");
            return;
        }

        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;

        // Baþlangýç ve bitiþ arasýndaki vektör ve mesafe hesaplanýr
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;
        Vector3 normalizedDirection = direction.normalized;

        // Kaç adet dað yerleþtirileceði hesaplanýr
        int count = Mathf.FloorToInt(distance / spacing);

        // Dönüþ açýsýný (rotation) hesaplama
        Quaternion rotation = Quaternion.LookRotation(normalizedDirection);

        // Yerleþtirme Döngüsü
        for (int i = 0; i <= count; i++)
        {
            // Yeni pozisyon hesaplanýr
            Vector3 placementPos = startPos + normalizedDirection * (i * spacing);

            // Rastgele döndürme (doðallýk katmak için)
            Quaternion randomRotation = rotation * Quaternion.Euler(0, Random.Range(-10f, 10f), 0);

            // Prefab'ý sahnede oluþtur
            GameObject newMountain = (GameObject)PrefabUtility.InstantiatePrefab(mountainPrefab);

            // Pozisyon ve Dönüþ açýsý atanýr
            newMountain.transform.position = placementPos;
            newMountain.transform.rotation = randomRotation;

            // Undo/Redo (Geri Al/Yinele) için kayýt
            Undo.RegisterCreatedObjectUndo(newMountain, "Place Mountain");

            // Oluþan objeleri hiyerarþide bir Parent altýna koymak faydalý olabilir
            // newMountain.transform.SetParent(startPoint.parent); 
        }

        Debug.Log(count + " adet Dað yerleþtirildi.");
    }
}