using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AnimalArchiveEntry
{
    public string animalName;
    [TextArea(2, 4)] public string quickSummary;
    [TextArea(2, 4)] public string dailyCare;
    [TextArea(2, 4)] public string feeding;
    [TextArea(2, 4)] public string enrichment;
    [TextArea(2, 4)] public string health;

    public string BuildBody()
    {
        return "Quick Summary\n" + quickSummary + "\n\n" +
               "Daily Care\n" + dailyCare + "\n\n" +
               "Feeding\n" + feeding + "\n\n" +
               "Enrichment\n" + enrichment + "\n\n" +
               "Health\n" + health;
    }
}

[ExecuteAlways]
public class ArchiveAnimalLibraryPrefab : MonoBehaviour
{
    [Header("Panel Text")]
    [SerializeField] private string archiveTitle = "Pet Care Archive";
    [SerializeField] [TextArea(2, 4)] private string archiveSubtitle = "Point and click an animal button to learn how to raise and care for that pet.";

    [Header("Animals")]
    [SerializeField] private List<AnimalArchiveEntry> animalEntries = new List<AnimalArchiveEntry>();
    [SerializeField] private int defaultAnimalIndex = 0;

    private const string GeneratedPrefix = "__ArchiveGenerated_";
    private const string ButtonsViewportName = "ArchiveButtonsViewport";
    private const string ButtonsContentName = "ArchiveButtonsContent";
    private const string InfoViewportName = "ArchiveInfoViewport";
    private Text bodyText;
    private Transform interfaceCanvas;
    private Transform buttonsContainer;
    private Transform buttonsContent;
    private GameObject infoViewportObject;
    private readonly List<Button> generatedButtons = new List<Button>();
    private int currentIndex;
    private int openIndex = -1;
    private bool infoVisible;

    private void Awake()
    {
        InitializeOrRefresh();
    }

    private void OnEnable()
    {
        InitializeOrRefresh();
    }

    private void OnValidate()
    {
        InitializeOrRefresh();
    }

    public void ShowAnimalByIndex(int index)
    {
        if (animalEntries.Count == 0 || bodyText == null)
        {
            return;
        }

        currentIndex = Mathf.Clamp(index, 0, animalEntries.Count - 1);
        AnimalArchiveEntry entry = animalEntries[currentIndex];
        bodyText.text = entry.animalName + "\n\n" + entry.BuildBody();
        RefreshButtonVisuals();
    }

    private void InitializeOrRefresh()
    {
        EnsureDefaultEntries();
        ApplyPanelText();
        HideUnusedUi();
        ConfigureArchiveLayout();
        RebuildAnimalButtons();

        if (animalEntries.Count > 0)
        {
            int index = Mathf.Clamp(defaultAnimalIndex, 0, animalEntries.Count - 1);
            ShowAnimalByIndex(index);
            openIndex = index;
            SetInfoVisible(true);
        }
        else
        {
            SetInfoVisible(false);
        }
    }

    private void ApplyPanelText()
    {
        Text title = FindTextByName("Header Text");
        if (title != null)
        {
            title.text = archiveTitle;
        }

        Text subtitle = FindTextByName("Header Text (1)");
        if (subtitle != null)
        {
            subtitle.text = archiveSubtitle;
        }

        Text buttonsHeader = FindTextByExactContent("Buttons");
        if (buttonsHeader != null)
        {
            buttonsHeader.text = "Animals";
        }

        bodyText = FindTextByName("Modal Text");
        if (bodyText != null)
        {
            bodyText.raycastTarget = false;
            bodyText.alignment = TextAnchor.UpperLeft;
            bodyText.horizontalOverflow = HorizontalWrapMode.Wrap;
            bodyText.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }

    private void HideUnusedUi()
    {
        SetActiveByName("Toggles", false);
        SetActiveByName("Cont Move Toggle", false);
        SetActiveByName("Teleportation Toggle", false);
        SetActiveByName("Turn Dropdown", false);
        SetActiveByName("MinMaxSlider", false);
        SetActiveByName("Icon Toggle", false);
        SetActiveByName("Movement Toggles Text", false);
        SetActiveByName("Turn Dropdown Modal Text", false);
        SetActiveByName("Slider", false);

        SetActiveByName("Buttons", true, onlyButtonContainer: true);
        SetActiveByName("Modal Text", true);
    }

    private void ConfigureArchiveLayout()
    {
        interfaceCanvas = FindDescendant(transform, "Interface Canvas");
        buttonsContainer = FindButtonsContainer();

        if (interfaceCanvas == null || buttonsContainer == null || bodyText == null)
        {
            return;
        }

        RectTransform buttonsRect = buttonsContainer.GetComponent<RectTransform>();
        if (buttonsRect != null)
        {
            buttonsRect.anchorMin = new Vector2(0f, 1f);
            buttonsRect.anchorMax = new Vector2(0f, 1f);
            buttonsRect.pivot = new Vector2(0f, 1f);
            buttonsRect.anchoredPosition = new Vector2(12f, -132f);
            buttonsRect.sizeDelta = new Vector2(150f, 246f);
        }

        Image buttonsBg = GetOrAdd<Image>(buttonsContainer.gameObject);
        buttonsBg.color = new Color(0f, 0f, 0f, 0.14f);

        ScrollRect buttonScroll = GetOrAdd<ScrollRect>(buttonsContainer.gameObject);
        buttonScroll.horizontal = false;
        buttonScroll.vertical = true;
        buttonScroll.movementType = ScrollRect.MovementType.Clamped;
        buttonScroll.scrollSensitivity = 25f;

        Transform viewport = EnsureChild(buttonsContainer, ButtonsViewportName);
        RectTransform viewportRect = viewport.GetComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(6f, 6f);
        viewportRect.offsetMax = new Vector2(-6f, -6f);
        GetOrAdd<RectMask2D>(viewport.gameObject);

        buttonsContent = EnsureChild(viewport, ButtonsContentName);
        RectTransform buttonsContentRect = buttonsContent.GetComponent<RectTransform>();
        buttonsContentRect.anchorMin = new Vector2(0f, 1f);
        buttonsContentRect.anchorMax = new Vector2(1f, 1f);
        buttonsContentRect.pivot = new Vector2(0.5f, 1f);
        buttonsContentRect.anchoredPosition = Vector2.zero;
        buttonsContentRect.sizeDelta = new Vector2(0f, 0f);

        VerticalLayoutGroup vertical = GetOrAdd<VerticalLayoutGroup>(buttonsContent.gameObject);
        vertical.spacing = 8f;
        vertical.padding = new RectOffset(0, 0, 0, 0);
        vertical.childControlWidth = true;
        vertical.childControlHeight = true;
        vertical.childForceExpandWidth = true;
        vertical.childForceExpandHeight = false;

        ContentSizeFitter buttonsFitter = GetOrAdd<ContentSizeFitter>(buttonsContent.gameObject);
        buttonsFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        buttonsFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        buttonScroll.viewport = viewportRect;
        buttonScroll.content = buttonsContentRect;

        Transform template = FindDescendant(buttonsContainer, "Text Button");
        if (template != null)
        {
            template.SetParent(buttonsContent, false);
            template.gameObject.SetActive(false);
        }

        Transform icon = FindDescendant(buttonsContainer, "Icon Button");
        if (icon != null)
        {
            icon.SetParent(buttonsContent, false);
            icon.gameObject.SetActive(false);
        }

        infoViewportObject = EnsureChild(interfaceCanvas, InfoViewportName).gameObject;
        RectTransform infoRect = infoViewportObject.GetComponent<RectTransform>();
        infoRect.anchorMin = new Vector2(0f, 1f);
        infoRect.anchorMax = new Vector2(0f, 1f);
        infoRect.pivot = new Vector2(0f, 1f);
        infoRect.anchoredPosition = new Vector2(170f, -132f);
        infoRect.sizeDelta = new Vector2(192f, 246f);

        Image infoBg = GetOrAdd<Image>(infoViewportObject);
        infoBg.color = new Color(0f, 0f, 0f, 0.16f);

        RectMask2D infoMask = GetOrAdd<RectMask2D>(infoViewportObject);
        infoMask.padding = Vector4.zero;

        ScrollRect infoScroll = GetOrAdd<ScrollRect>(infoViewportObject);
        infoScroll.horizontal = false;
        infoScroll.vertical = true;
        infoScroll.movementType = ScrollRect.MovementType.Clamped;
        infoScroll.scrollSensitivity = 25f;

        RectTransform textRect = bodyText.GetComponent<RectTransform>();
        textRect.SetParent(infoViewportObject.transform, false);
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.pivot = new Vector2(0.5f, 1f);
        textRect.anchoredPosition = new Vector2(0f, -6f);
        textRect.sizeDelta = new Vector2(-12f, 0f);

        ContentSizeFitter textFitter = GetOrAdd<ContentSizeFitter>(bodyText.gameObject);
        textFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        textFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        LayoutElement textLayout = GetOrAdd<LayoutElement>(bodyText.gameObject);
        textLayout.minHeight = 0f;
        textLayout.preferredHeight = -1f;

        infoScroll.viewport = infoRect;
        infoScroll.content = textRect;
        infoScroll.verticalNormalizedPosition = 1f;
    }

    private void RebuildAnimalButtons()
    {
        if (buttonsContainer == null || buttonsContent == null)
        {
            return;
        }

        Button templateButton = FindDescendant(buttonsContent, "Text Button")?.GetComponent<Button>();
        if (buttonsContainer == null)
        {
            return;
        }

        if (templateButton == null)
        {
            return;
        }

        templateButton.gameObject.SetActive(false);
        ClearGeneratedButtons(buttonsContent);
        generatedButtons.Clear();

        for (int i = 0; i < animalEntries.Count; i++)
        {
            int capturedIndex = i;
            GameObject instance = Instantiate(templateButton.gameObject, buttonsContent, false);
            instance.name = GeneratedPrefix + animalEntries[i].animalName;
            instance.SetActive(true);

            Text label = FindDescendant(instance.transform, "Text")?.GetComponent<Text>();
            if (label != null)
            {
                label.text = animalEntries[i].animalName;
            }

            RectTransform rect = instance.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 46f);
            }

            LayoutElement layout = GetOrAdd<LayoutElement>(instance);
            layout.preferredHeight = 46f;
            layout.flexibleHeight = 0f;

            Button button = instance.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => ToggleAnimalInfo(capturedIndex));
                generatedButtons.Add(button);
            }
        }
    }

    private void ToggleAnimalInfo(int index)
    {
        if (infoVisible && openIndex == index)
        {
            openIndex = -1;
            infoVisible = false;
            SetInfoVisible(false);
            RefreshButtonVisuals();
            return;
        }

        ShowAnimalByIndex(index);
        openIndex = index;
        infoVisible = true;
        SetInfoVisible(true);
    }

    private void RefreshButtonVisuals()
    {
        for (int i = 0; i < generatedButtons.Count; i++)
        {
            Image img = generatedButtons[i] != null ? generatedButtons[i].GetComponent<Image>() : null;
            if (img == null)
            {
                continue;
            }

            bool isSelected = infoVisible && i == currentIndex;
            img.color = isSelected
                ? new Color(0.31f, 0.62f, 0.89f, 1f)
                : new Color(0.125f, 0.588f, 0.953f, 1f);
        }
    }

    private void ClearGeneratedButtons(Transform buttonsContainer)
    {
        List<Transform> toDelete = new List<Transform>();
        for (int i = 0; i < buttonsContainer.childCount; i++)
        {
            Transform child = buttonsContainer.GetChild(i);
            if (child.name.StartsWith(GeneratedPrefix, StringComparison.Ordinal))
            {
                toDelete.Add(child);
            }
        }

        for (int i = 0; i < toDelete.Count; i++)
        {
            if (Application.isPlaying)
            {
                Destroy(toDelete[i].gameObject);
            }
            else
            {
                DestroyImmediate(toDelete[i].gameObject);
            }
        }
    }

    private Transform FindButtonsContainer()
    {
        Transform[] all = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].name != "Buttons")
            {
                continue;
            }

            if (FindDescendant(all[i], "Text Button") != null)
            {
                all[i].gameObject.SetActive(true);
                return all[i];
            }
        }

        return null;
    }

    private void SetInfoVisible(bool visible)
    {
        if (infoViewportObject != null)
        {
            infoViewportObject.SetActive(visible);
        }

        if (bodyText != null && !visible)
        {
            bodyText.text = string.Empty;
        }
    }

    private static T GetOrAdd<T>(GameObject go) where T : Component
    {
        T existing = go.GetComponent<T>();
        if (existing != null)
        {
            return existing;
        }

        return go.AddComponent<T>();
    }

    private static Transform EnsureChild(Transform parent, string childName)
    {
        Transform existing = FindDescendant(parent, childName);
        if (existing != null)
        {
            return existing;
        }

        GameObject child = new GameObject(childName, typeof(RectTransform));
        child.transform.SetParent(parent, false);
        return child.transform;
    }

    private void SetActiveByName(string objectName, bool active, bool onlyButtonContainer = false)
    {
        Transform[] all = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].name != objectName)
            {
                continue;
            }

            if (onlyButtonContainer && FindDescendant(all[i], "Text Button") == null)
            {
                continue;
            }

            all[i].gameObject.SetActive(active);
        }
    }

    private Text FindTextByName(string objectName)
    {
        Transform t = FindDescendant(transform, objectName);
        return t != null ? t.GetComponent<Text>() : null;
    }

    private Text FindTextByExactContent(string content)
    {
        Text[] all = GetComponentsInChildren<Text>(true);
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i] != null && all[i].text == content)
            {
                return all[i];
            }
        }

        return null;
    }

    private static Transform FindDescendant(Transform root, string objectName)
    {
        if (root == null)
        {
            return null;
        }

        if (root.name == objectName)
        {
            return root;
        }

        for (int i = 0; i < root.childCount; i++)
        {
            Transform result = FindDescendant(root.GetChild(i), objectName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private void EnsureDefaultEntries()
    {
        if (animalEntries.Count > 0)
        {
            return;
        }

        animalEntries.Add(new AnimalArchiveEntry
        {
            animalName = "Cat",
            quickSummary = "Cats are independent, curious pets that still need daily social time and environmental enrichment.",
            dailyCare = "Scoop litter daily, refresh water, and schedule at least 15-20 minutes of interactive play.",
            feeding = "Feed measured portions 2-3 times per day and avoid overfeeding to prevent obesity.",
            enrichment = "Use climbing shelves, scratch posts, puzzle feeders, and rotating toys to reduce boredom.",
            health = "Watch for appetite changes, vomiting, hiding, or litter box issues and keep annual vet checkups."
        });

        animalEntries.Add(new AnimalArchiveEntry
        {
            animalName = "Dog",
            quickSummary = "Dogs need structure, exercise, and consistent training to thrive in a home environment.",
            dailyCare = "Walk at least twice per day, provide bathroom breaks, and practice short training sessions.",
            feeding = "Use age-appropriate food, controlled portions, and fresh water all day.",
            enrichment = "Provide sniff walks, chew toys, socialization, and basic command games.",
            health = "Maintain vaccines, parasite prevention, dental care, and monitor energy or behavior changes."
        });

        animalEntries.Add(new AnimalArchiveEntry
        {
            animalName = "Rabbit",
            quickSummary = "Rabbits are social prey animals that require quiet spaces, routine, and gentle handling.",
            dailyCare = "Clean litter areas daily, provide floor time, and keep a calm environment.",
            feeding = "Unlimited hay, daily leafy greens, and controlled pellets are the core diet.",
            enrichment = "Offer tunnels, dig boxes, chew-safe items, and safe hide spots.",
            health = "Rabbits hide illness, so reduced appetite, low droppings, or lethargy needs immediate vet care."
        });

        animalEntries.Add(new AnimalArchiveEntry
        {
            animalName = "Parakeet",
            quickSummary = "Parakeets are intelligent birds that need mental stimulation and consistent social interaction.",
            dailyCare = "Refresh food and water, clean perches, and provide out-of-cage time when safe.",
            feeding = "Use balanced pellets with seed as a supplement plus safe vegetables.",
            enrichment = "Rotate perches and toys and include sound or target training for engagement.",
            health = "Look for posture, feather, appetite, and droppings changes; birds decline quickly when sick."
        });

        animalEntries.Add(new AnimalArchiveEntry
        {
            animalName = "Goldfish",
            quickSummary = "Goldfish need spacious, filtered tanks and stable water quality, not small bowls.",
            dailyCare = "Check temperature and filter flow, remove debris, and observe fish behavior.",
            feeding = "Feed small portions once or twice daily and avoid excess food that pollutes water.",
            enrichment = "Provide swimming room, mild decor, and low-stress tankmates if compatible.",
            health = "Test water weekly for ammonia, nitrite, nitrate, and address illness early."
        });
    }
}
