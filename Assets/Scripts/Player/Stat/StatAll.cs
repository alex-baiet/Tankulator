using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions.Must;
//NewScript

public class StatAll : MonoBehaviour
{
    public static Dictionary<string, Stat> stats = new Dictionary<string, Stat>();

    private bool noError = true;


    void Awake()
    {

        LoadStatAll();
    }

    public static void CreateLog(string error)
    {

        if (!Directory.Exists("Logs")) { Directory.CreateDirectory("Logs"); }
        if (!File.Exists("Logs/log.log")) { File.WriteAllText("Logs/log.log", error + "\n"); }
        else { File.AppendAllText("Logs/log.log", error + "\n"); }
        Debug.LogError(error);
    }

    /// <summary>
    /// Crée/écrit et sauvegarde dans un fichier JSON dans "Mods" les varibles d'une classe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">nom du fichier cible.</param>
    /// <param name="stat">classe de valeurs a sauvegarder.</param>
    public static void SaveStat<T>(string fileName, T stat)
    {
        if (!fileName.Contains(".json")) { fileName += ".json"; }
        string json = JsonUtility.ToJson(stat);
        if (!Directory.Exists("mods")) { Directory.CreateDirectory("Mods"); }
        File.WriteAllText("mods/" + fileName, json);
    }

    /// <summary>
    /// Retourne dans la classe indiquée le contenu d'un fichier JSON.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">Nom du fichier JSON dans "Mods/".</param>
    /// <returns></returns>
    public static T LoadStat<T>(string fileName)
    {
        if (!fileName.Contains(".json")) { fileName += ".json"; }
        //Debug.Log("LoadStat : " + fileName);
        string json = File.ReadAllText(fileName);
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// Charge toutes les stats.
    /// </summary>
    private void LoadStatAll()
    {
        stats.Clear();

        foreach (string dir in Directory.GetDirectories("Mods/Stats"))
        {
            try
            {
                noError = true;

                //Debug.Log(dir);
                Stat newStat = new Stat();
                StatPlayerLoaded p = null;
                Dictionary<string, StatSpecialLoaded> sp = new Dictionary<string, StatSpecialLoaded>();
                Dictionary<string, StatWeaponLoaded> w = new Dictionary<string, StatWeaponLoaded>();
                Dictionary<string, StatBulletLoaded> b = new Dictionary<string, StatBulletLoaded>();
                newStat.path = dir.Replace(@"Mods/Stat\", "");

                void LoadAllJsonFile(string path)
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        try
                        {
                            if (file.EndsWith("player.json"))
                            {
                                if (p == null) { p = LoadStat<StatPlayerLoaded>(file); }
                                else { 
                                    CreateLog(dir + " : Plusieurs fichiers '.player.json' ont été trouvés. Veuillez n'en laissez qu'un par personnages.");
                                    noError = false;
                                }
                            }
                            else if (file.EndsWith("special.json"))
                            {
                                StatSpecialLoaded newS = LoadStat<StatSpecialLoaded>(file);
                                sp.Add(newS.name, newS);
                            }
                            else if (file.EndsWith("weapon.json"))
                            {
                                StatWeaponLoaded newS = LoadStat<StatWeaponLoaded>(file);
                                w.Add(newS.name, newS);
                            }
                            else if (file.EndsWith("bullet.json"))
                            {
                                StatBulletLoaded newS = LoadStat<StatBulletLoaded>(file);
                                b.Add(newS.name, newS);
                            }
                        }
                        catch
                        {
                            CreateLog(path + " : Le fichier n'a pas pu être chargée. Son contenue est peut-être mal structuré.");
                            noError = false;
                        }
                    }
                    foreach (string underDir in Directory.GetDirectories(path))
                    {
                        LoadAllJsonFile(underDir);
                    }
                }

                LoadAllJsonFile(dir);
                if (!noError) { goto next; }

                //StatPlayer
                if (p != null) { newStat.player = p; }
                else { CreateLog(dir + " : Le joueur " + newStat.player.name + " n'a pas pu être chargé."); goto next; }
                
                if (Directory.Exists(dir + "/" + newStat.player.spritePath))
                {
                    newStat.player.anim = new AnimationMod(dir + "/" + newStat.player.spritePath, newStat.player.spritePath);
                    newStat.player.texture = newStat.player.anim.GetFirstSprite().texture;
                }
                else if (File.Exists(dir + "/" + newStat.player.spritePath))
                {
                    newStat.player.sprite = LoadSpriteFromFile(dir + "/" + newStat.player.spritePath);
                    if (newStat.player.sprite != null) { newStat.player.texture = LoadTextureFromFile(dir + "/" + newStat.player.spritePath); }
                    else { goto next; }
                }
                else { CreateLog(newStat.player.spritePath + " : L'image ou l'animation du personnage n'existe pas."); goto next; }

                if (!sp.ContainsKey(newStat.player.specialName))
                {
                    CreateLog(dir + " : Le spécial " + newStat.player.specialName + " du personnage " + newStat.player.name + " n'existe pas dans le dossier.");
                    goto next;
                }
                foreach (string weaponName in newStat.player.weaponsName)
                {
                    if (!w.ContainsKey(weaponName))
                    {
                        CreateLog(dir + " : L'arme " + weaponName + " du personnage " + newStat.player.name + " n'existe pas dans le dossier.");
                        goto next;
                    }
                }

                //StatSpecial
                newStat.specials = sp;
                foreach (KeyValuePair<string, StatSpecialLoaded> pairSp in newStat.specials)
                {
                    pairSp.Value.anim = new AnimationMod(dir + "/" + pairSp.Value.animPath, pairSp.Value.animPath);
                    pairSp.Value.animHover = new AnimationMod(dir + "/" + pairSp.Value.animHoverPath, pairSp.Value.animHoverPath);
                }

                //StatWeapon
                newStat.weapons = w;
                foreach (KeyValuePair<string, StatWeaponLoaded> pairWeapon in newStat.weapons)
                {
                    if (pairWeapon.Value.ammo.Length == 0)
                    {
                        CreateLog(dir + " : L'arme " + pairWeapon.Value.name + "n'a pas de balle.");
                        goto next;
                    }
                    try
                    {
                        for (int j = 0; j < pairWeapon.Value.ammo.Length; j++)
                        {
                            string[] bulletName = pairWeapon.Value.ammo[j].Split('*');
                            if (bulletName.Length == 1) { pairWeapon.Value.bullets.Add(bulletName[0]); }
                            else { for (int k = 0; k < int.Parse(bulletName[1]); k++) { pairWeapon.Value.bullets.Add(bulletName[0]); } }

                            if (!b.ContainsKey(bulletName[0]))
                            {
                                CreateLog(dir + " : le dossier ne contient pas la balle " + bulletName[0] + " pour l'arme " + pairWeapon.Value.name + ".");
                                goto next;
                            }
                        }
                    }
                    catch
                    {
                        CreateLog(dir + " : Le nom des balles de l'arme " + pairWeapon.Value.name + " n'est pas écrit correctement.");
                        goto next;
                    }

                    pairWeapon.Value.shootAnim = new AnimationMod(dir + "/" + pairWeapon.Value.shootAnimPath, pairWeapon.Value.shootAnimPath);
                }

                //StatBullet
                newStat.bullets = b;
                foreach (KeyValuePair<string, StatBulletLoaded> pairBullet in newStat.bullets)
                {
                    if (pairBullet.Value.spritePath != "" || pairBullet.Value.spritePath != null)
                    {
                        if (File.Exists(dir + "/" + pairBullet.Value.spritePath))
                        {
                            pairBullet.Value.sprite = LoadSpriteFromFile(dir + "/" + pairBullet.Value.spritePath);
                        }
                        else if (Directory.Exists(dir + "/" + pairBullet.Value.spritePath))
                        {
                            pairBullet.Value.anim = new AnimationMod(dir + "/" + pairBullet.Value.spritePath, pairBullet.Value.spritePath);
                        }
                        else
                        {
                            CreateLog(pairBullet.Value.spritePath + " : L'image ou le dossier d'animation de la balle "
                                + pairBullet.Value.name + " n'exste pas. " +
                                "Si vous ne voulez pas d'image pour la balle, laissez le paramètre pour le chemin vide.");
                            goto next;
                        }
                    }

                    pairBullet.Value.destroyedAnim = new AnimationMod(dir + "/" + pairBullet.Value.destroyedAnimPath, pairBullet.Value.destroyedAnimPath);

                }
                //Stat ajout
                if (noError)
                {
                    if (!stats.ContainsKey(newStat.player.name)) { stats.Add(newStat.player.name, newStat); }
                }
            }
            catch (Exception)
            {
                CreateLog(dir + " : Le dossier n'a pas put être chargé à cause d'une erreur inconnu.");
            }
        next: continue;
        }
    }

    public static Sprite LoadSpriteFromFile(string path)
    {
        return LoadSpriteFromFile(path, 24f);
    }

    //Peut être raccourcie avec LoadTextureFromFile.
    public static Sprite LoadSpriteFromFile(string path, float pixelPerUnit)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            texture.filterMode = FilterMode.Point;
            //Debug.Log(texture.width);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelPerUnit);
            return sprite;
        }
        catch
        {
            CreateLog(path + " : L'image n'a pas pu être chargée.");
            return null;
        }
    }

    public static Texture2D LoadTextureFromFile(string path)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            texture.filterMode = FilterMode.Point;
            return texture;
        }
        catch
        {
            CreateLog(path + " : L'image n'a pas pu être chargée.");
            return null;
        }
    }

    public static float MathDistance(Vector3 pos1, Vector3 pos2)
    {
        float d = Mathf.Sqrt(
        Mathf.Pow(pos1.x - pos2.x, 2) +
        Mathf.Pow(pos1.y - pos2.y, 2));

        return d;
    }
}
