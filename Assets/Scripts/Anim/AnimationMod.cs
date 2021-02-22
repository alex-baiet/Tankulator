using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//NewScript

public class AnimationMod
{
    

    public bool enable = true;
    public Dictionary<float, Sprite> sprites = new Dictionary<float, Sprite>();
    public List<float> spritesKeys = new List<float>();

    private SpriteRenderer renderer;
    private Sprite defaultSprite = null;
    private string pathAnim;

    private float clock = 0f;
    private float lastAIinstance = 0f;
    private AfterimageVar AIVar = null;
    private List<SpriteRenderer> AI = new List<SpriteRenderer>();
    private List<float> AITime = new List<float>();
    private Transform afterimageTransform;
    private GameObject emptySprite;

    private ParticleVar particleVar = null;
    private Texture2D particleTexture = null;
    private ParticleSystem particle = null;
    
    private float timer = 0f;
    private int actualSpriteKey;
    private bool animating = false;
    private bool repeat = false;
    private string timeName = null;
    private bool wasPaused = false;

    public AnimationMod(string pathAnimation, string animName)
    {
        if (animName != "" && animName != null)
        {
            try
            {
                //Debug.Log(pathAnimation);
                pathAnim = pathAnimation;
                emptySprite = Resources.Load<GameObject>("Renderer");

                if (Directory.Exists(pathAnim))
                {
                    foreach (string fileName in Directory.GetFiles(pathAnim))
                    {
                        try
                        {
                            KeyValuePair<float, Sprite> pairSprite;
                            if (fileName.Contains("afterimage.json"))
                            {
                                AIVar = StatAll.LoadStat<AfterimageVar>(fileName);
                            }
                            else if (fileName.Contains("particle.json"))
                            {
                                particleVar = StatAll.LoadStat<ParticleVar>(fileName);
                            }
                            else if (fileName.Contains("particle"))
                            {
                                particleTexture = StatAll.LoadTextureFromFile(fileName);
                            }
                            else
                            {
                                pairSprite = CreatePairSprite(fileName);
                                sprites.Add(pairSprite.Key, pairSprite.Value);
                                spritesKeys.Add(pairSprite.Key);
                            }
                        }
                        catch { 
                            StatAll.CreateLog(fileName + " : Le fichier d'animation n'a pas pu être chargé.");
                            break;
                        }
                    }
                    spritesKeys.Sort();
                }
                else
                {
                    Debug.LogError(pathAnim + " : Le dossier d'animation n'existe pas.");
                    enable = false;
                }
            }
            catch
            {
                StatAll.CreateLog(pathAnim + " : L'animation n'a pas pu être chargée pour une raison inconnu.");
                enable = false;
            }
        }
        else
        {
            enable = false;
        }

        if (!spritesKeys.Contains(0f) && spritesKeys.Count > 0)
        {
            StatAll.CreateLog(pathAnim + " : L'animation ne contient pas d'image '0.png'. Il est donc désactivé.");
        }
    }

    public AnimationMod()
    {
        enable = false;
    }

    public AnimationMod(AnimationMod animToCopy)
    {
        emptySprite = Resources.Load<GameObject>("Renderer");

        afterimageTransform = GameObject.Find("Afterimages").transform;
        sprites = animToCopy.sprites;
        spritesKeys = animToCopy.spritesKeys;
        enable = animToCopy.enable;
        AIVar = animToCopy.AIVar;
        particleVar = animToCopy.particleVar;
        particleTexture = animToCopy.particleTexture;
    }

    public bool CheckAnimationEnded() { return !enable || !animating; }

    public bool CheckIsEnable() { return enable; }

    private KeyValuePair<float, Sprite> CreatePairSprite(string fileName)
    {
        Sprite newSprite = StatAll.LoadSpriteFromFile(fileName);

        float newMeasure;

        string measureName = "";
        string spriteName = fileName.Replace(pathAnim + @"\", null);
        for (int i = 0; i < spriteName.LastIndexOf('.'); i++)
        {
            measureName += spriteName[i];
        }

        if (!float.TryParse(measureName, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out newMeasure))
        {
            Debug.LogError("L'image d'animation " + fileName + " a été mal nommée.");
        }

        return new KeyValuePair<float, Sprite>(newMeasure, newSprite);
    }

    public Sprite GetFirstSprite()
    {
        return sprites.Count > 0 ? sprites[spritesKeys[0]] : null;
    }

    public void UpdateAfterimages()
    {
        clock += Time.deltaTime * Clock.GetTimeFlow(timeName);
        
        if (animating && enable && renderer != null && AIVar != null)
        {
            if (lastAIinstance < clock - AIVar.interval)
            {
                lastAIinstance = clock;
                GameObject newAI = Object.Instantiate(emptySprite, renderer.transform.position, renderer.transform.rotation, afterimageTransform);
                newAI.AddComponent<Afterimage>();
                newAI.GetComponent<Afterimage>().Init(timeName, AIVar, renderer);
            }
        }
    }

    public void UpdateAnimation()
    {
        if (animating && enable && renderer != null && spritesKeys.Count > 0)
        {
            timer += Time.deltaTime * Clock.GetTimeFlow(timeName);

            while (spritesKeys[actualSpriteKey] <= timer)
            {
                renderer.sprite = sprites[spritesKeys[actualSpriteKey]];
                actualSpriteKey++;
                
                if (actualSpriteKey == spritesKeys.Count)
                {
                    actualSpriteKey = 0;
                    animating = repeat;
                    if (repeat) { timer -= spritesKeys[spritesKeys.Count - 1]; }
                    else { 
                        timer = 0; 
                        renderer.sprite = defaultSprite;
                        break; 
                    }
                }

                if (spritesKeys.Count == 1) { break; }
            }
        }

        UpdateAfterimages();
        UpdateParticle();
    }

    private void UpdateParticle()
    {
        if (enable && particle != null)
        {
            if (wasPaused != Clock.isPaused)
            {
                if (Clock.isPaused)
                {
                    particle.Pause();
                }
            }
            wasPaused = Clock.isPaused;

            if (!Clock.isPaused)
            {
                if (animating)
                {
                    if (particle.isStopped || particle.isPaused)
                    {
                        particle.Play();
                    }
                }
                else if (particle.isPlaying)
                {
                    particle.Stop();
                }
            }
        }
    }

    public void Init(SpriteRenderer objectRenderer, Sprite defaultSprite, string timeName) 
    { 
        renderer = objectRenderer;
        this.defaultSprite = defaultSprite;
        afterimageTransform = GameObject.Find("Afterimages").transform;
        this.timeName = timeName;

        if (particleVar != null)
        {
            InitParticle();
        }
    }

    public void InitParticle()
    {
        if (particleTexture != null)
        {
            renderer.gameObject.AddComponent<ParticleSystem>();
            particle = renderer.gameObject.GetComponent<ParticleSystem>();
            ParticleSystemRenderer particleRenderer = renderer.gameObject.GetComponent<ParticleSystemRenderer>();
            Material material = new Material(Shader.Find("Unlit/Transparent"));
            ParticleSystem.MainModule mainModule = particle.main;
            ParticleSystem.ShapeModule shapeModule = particle.shape;
            ParticleSystem.EmissionModule emissionModule = particle.emission;

            material.mainTexture = particleTexture;
            particleRenderer.material = material;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;
            shapeModule.shapeType = ParticleSystemShapeType.Circle;
            mainModule.startSize3D = true;
            mainModule.startSizeX = particleTexture.width / 24f;
            mainModule.startSizeY = particleTexture.height / 24f;

            shapeModule.radius = particleVar.radius;
            shapeModule.radiusThickness = particleVar.radiusThickness;
            shapeModule.randomDirectionAmount = particleVar.randomDirectionAmount;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(particleVar.rate);
            mainModule.startSpeed = new ParticleSystem.MinMaxCurve(particleVar.speed);
            mainModule.startLifetime = new ParticleSystem.MinMaxCurve(particleVar.lifetime);

            particle.Stop();
        }
        else
        {
            StatAll.CreateLog(pathAnim + " : L'animation a 'particle.json' mais n'a pas 'particle.png'. Le système de particules ne peut pas être créé.");
        }
    }

    public void StartAnimation()
    {
        if (enable)
        {
            animating = true;
            if (particle != null) { particle.Play(); }
        }
    }
    public void StartRepeatingAnimation() {
        if (enable)
        {
            animating = true; repeat = true;
            if (particle != null) { particle.Play(); }
        }
    }
    public void StopAnimation() {
        if (enable)
        {
            animating = false;
            repeat = false;
            actualSpriteKey = 0;
            timer = 0;
            renderer.sprite = defaultSprite;
            if (particle != null) { particle.Stop(); }
        }
    }
    public void StopRepeatingAnimation() {
        if (enable)
        {
            repeat = false;
        }
    }

}
