using com.guidepilot.guidepilotcore;

public static class SceneExtension
{
    public static string SceneName(this SceneId sceneId)
    {
        switch (sceneId)
        {
            case SceneId.NietzscheGameBoot:
                return "Nietzsche_Game_Boot";

            case SceneId.NietzscheGameMain:
                return "Nietzsche_Game_Main";

            case SceneId.IlmparkGameBoot:
                return "Ilmpark_Boot";

            case SceneId.IlmparkGameAr:
                return "Ilmpark_AR";

            case SceneId.IlmparkGameMain:
                return "Ilmpark_Main";

            case SceneId.ViewerBoot:
                return "3DViewer_Boot";

            case SceneId.ViewerArBoot:
                return "ARViewer_Boot";

            case SceneId.NietzscheDeathChamber_1903:
                return "Nietzsche_DeathChamber_1903";

            case SceneId.NietzscheDeathChamber_1926:
                return "Nietzsche_DeathChamber_1926";

            case SceneId.NietzscheDeathChamber_1956:
                return "Nietzsche_DeathChamber_1956";

            case SceneId.NietzscheDeathChamberToday:
                return "Nietzsche_DeathChamber_Today";

            case SceneId.AnnaAmaliaBooksSauerBible:
                return "AnnaAmalia_Books_SauerBible";

            case SceneId.AnnaAmaliaBooksJosephusopera:
                return "AnnaAmalia_Books_JosephusOpera";

            case SceneId.AnnaAmaliaBooksDeStatuReligionis:
                return "AnnaAmalia_Books_DeStatuReligionis";

            case SceneId.AnnaAmaliaBooksHistoireDeJulesCesar:
                return "AnnaAmalia_Books_HistoireDeJulesCesar";

            case SceneId.AnnaAmaliaBooksSynodus:
                return "AnnaAmalia_Books_Synodus";

            case SceneId.AnnaAmaliaBooksTabularium:
                return "AnnaAmalia_Books_Tabularium";

            case SceneId.NietzscheDeathChamberBoot:
                return "Nietzsche_DeathChamber_Boot";

            case SceneId.AnnaAmaliaBooksBoot:
                return "AnnaAmalia_Books_Boot";

            default:
                return "";
        }
    }

    public static SceneId SceneID(this string sceneName)
    {
        switch (sceneName)
        {
            case "Nietzsche_Game_Boot":
                return SceneId.NietzscheGameBoot;

            case "Nietzsche_Game_Main":
                return SceneId.NietzscheGameMain;

            case "Ilmpark_Boot":
                return SceneId.IlmparkGameBoot;

            case "Ilmpark_AR":
                return SceneId.IlmparkGameAr;

            case "Ilmpark_Main":
                return SceneId.IlmparkGameMain;

            case "3DViewer_Boot":
                return SceneId.ViewerBoot;

            case "ARViewer_Boot":
                return SceneId.ViewerArBoot;

            case "Nietzsche_DeathChamber_1903":
                return SceneId.NietzscheDeathChamber_1903;

            case "Nietzsche_DeathChamber_1926":
                return SceneId.NietzscheDeathChamber_1926;

            case "Nietzsche_DeathChamber_1956":
                return SceneId.NietzscheDeathChamber_1956;

            case "Nietzsche_DeathChamber_Today":
                return SceneId.NietzscheDeathChamberToday;

            case "AnnaAmalia_Books_SauerBible":
                return SceneId.AnnaAmaliaBooksSauerBible;

            case "AnnaAmalia_Books_JosephusOpera":
                return SceneId.AnnaAmaliaBooksJosephusopera;

            case "AnnaAmalia_Books_DeStatuReligionis":
                return SceneId.AnnaAmaliaBooksDeStatuReligionis;

            case "AnnaAmalia_Books_HistoireDeJulesCesar":
                return SceneId.AnnaAmaliaBooksHistoireDeJulesCesar;

            case "AnnaAmalia_Books_Synodus":
                return SceneId.AnnaAmaliaBooksSynodus;

            case "AnnaAmalia_Books_Tabularium":
                return SceneId.AnnaAmaliaBooksTabularium;

            case "Nietzsche_DeathChamber_Boot":
                return SceneId.NietzscheDeathChamberBoot;

            case "AnnaAmalia_Books_Boot":
                return SceneId.AnnaAmaliaBooksBoot;

            default:
                return default;
        }
    }
}
