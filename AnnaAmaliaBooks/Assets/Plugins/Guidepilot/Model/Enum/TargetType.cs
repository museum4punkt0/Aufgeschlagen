//
//  TargetType.cs
//  GuidePilot
//
//  Allowed targets in menus. Use only for MenuSection and MenuSectionItem. If
//  target_type is NONE, target_uuid is NULL. DISCOVER is deprecated and was
//  renamed to EXPLORE
//
//  Generated by GuidePilot - EnumGenerator
//  Copyright © 2021 MicroMovie Media GmbH. All rights reserved.


using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace com.guidepilot.guidepilotcore {
	[Preserve]
	public enum TargetType {
		None,
		Content,
		ContentUsage,
		ContentCompilation,
		ContentCompilationList,
		Map,
		Tour,
		TourList,
		Exhibition,
		ExhibitionList,
		ExhibitionZone,
		Exhibit,
		Actor,
		ActorList,
		ActorListOfCurrentExhibitions,
		ActorListOfCurrentContentCompilations,
		Discover,
		Explore,
		ExploreInteractiveMedia,
		Favorites,
		LanguageSelector,
		Menu,
		Document,
		Weblink,
		Custom,
		Ticketing,
		IndoorMap,
		RateApp,
		RecommendApp,
		OpenBookFeed,
		SoftwareLicenses,
		Search,
		Unity,
		CalendarView,
		CalendarEvent,
		Settings
	}

	[Preserve]
	public class TargetTypeConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			TargetType castedValue = (TargetType)value;
			switch (castedValue) {
			case TargetType.None:
			writer.WriteValue("NONE");break;
			case TargetType.Content:
			writer.WriteValue("CONTENT");break;
			case TargetType.ContentUsage:
			writer.WriteValue("CONTENT_USAGE");break;
			case TargetType.ContentCompilation:
			writer.WriteValue("CONTENT_COMPILATION");break;
			case TargetType.ContentCompilationList:
			writer.WriteValue("CONTENT_COMPILATION_LIST");break;
			case TargetType.Map:
			writer.WriteValue("MAP");break;
			case TargetType.Tour:
			writer.WriteValue("TOUR");break;
			case TargetType.TourList:
			writer.WriteValue("TOUR_LIST");break;
			case TargetType.Exhibition:
			writer.WriteValue("EXHIBITION");break;
			case TargetType.ExhibitionList:
			writer.WriteValue("EXHIBITION_LIST");break;
			case TargetType.ExhibitionZone:
			writer.WriteValue("EXHIBITION_ZONE");break;
			case TargetType.Exhibit:
			writer.WriteValue("EXHIBIT");break;
			case TargetType.Actor:
			writer.WriteValue("ACTOR");break;
			case TargetType.ActorList:
			writer.WriteValue("ACTOR_LIST");break;
			case TargetType.ActorListOfCurrentExhibitions:
			writer.WriteValue("ACTOR_LIST_OF_CURRENT_EXHIBITIONS");break;
			case TargetType.ActorListOfCurrentContentCompilations:
			writer.WriteValue("ACTOR_LIST_OF_CURRENT_CONTENT_COMPILATIONS");break;
			case TargetType.Discover:
			writer.WriteValue("DISCOVER");break;
			case TargetType.Explore:
			writer.WriteValue("EXPLORE");break;
			case TargetType.ExploreInteractiveMedia:
			writer.WriteValue("EXPLORE_INTERACTIVE_MEDIA");break;
			case TargetType.Favorites:
			writer.WriteValue("FAVORITES");break;
			case TargetType.LanguageSelector:
			writer.WriteValue("LANGUAGE_SELECTOR");break;
			case TargetType.Menu:
			writer.WriteValue("MENU");break;
			case TargetType.Document:
			writer.WriteValue("DOCUMENT");break;
			case TargetType.Weblink:
			writer.WriteValue("WEBLINK");break;
			case TargetType.Custom:
			writer.WriteValue("CUSTOM");break;
			case TargetType.Ticketing:
			writer.WriteValue("TICKETING");break;
			case TargetType.IndoorMap:
			writer.WriteValue("INDOOR_MAP");break;
			case TargetType.RateApp:
			writer.WriteValue("RATE_APP");break;
			case TargetType.RecommendApp:
			writer.WriteValue("RECOMMEND_APP");break;
			case TargetType.OpenBookFeed:
			writer.WriteValue("OPEN_BOOK_FEED");break;
			case TargetType.SoftwareLicenses:
			writer.WriteValue("SOFTWARE_LICENSES");break;
			case TargetType.Search:
			writer.WriteValue("SEARCH");break;
			case TargetType.Unity:
			writer.WriteValue("UNITY");break;
			case TargetType.CalendarView:
			writer.WriteValue("CALENDAR_VIEW");break;
			case TargetType.CalendarEvent:
			writer.WriteValue("CALENDAR_EVENT");break;
			case TargetType.Settings:
			writer.WriteValue("SETTINGS");break;
			}
		}

		public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer) {
			var enumString = (string)reader.Value;
			switch (enumString) {
			case "NONE":
			return TargetType.None;
			case "CONTENT":
			return TargetType.Content;
			case "CONTENT_USAGE":
			return TargetType.ContentUsage;
			case "CONTENT_COMPILATION":
			return TargetType.ContentCompilation;
			case "CONTENT_COMPILATION_LIST":
			return TargetType.ContentCompilationList;
			case "MAP":
			return TargetType.Map;
			case "TOUR":
			return TargetType.Tour;
			case "TOUR_LIST":
			return TargetType.TourList;
			case "EXHIBITION":
			return TargetType.Exhibition;
			case "EXHIBITION_LIST":
			return TargetType.ExhibitionList;
			case "EXHIBITION_ZONE":
			return TargetType.ExhibitionZone;
			case "EXHIBIT":
			return TargetType.Exhibit;
			case "ACTOR":
			return TargetType.Actor;
			case "ACTOR_LIST":
			return TargetType.ActorList;
			case "ACTOR_LIST_OF_CURRENT_EXHIBITIONS":
			return TargetType.ActorListOfCurrentExhibitions;
			case "ACTOR_LIST_OF_CURRENT_CONTENT_COMPILATIONS":
			return TargetType.ActorListOfCurrentContentCompilations;
			case "DISCOVER":
			return TargetType.Discover;
			case "EXPLORE":
			return TargetType.Explore;
			case "EXPLORE_INTERACTIVE_MEDIA":
			return TargetType.ExploreInteractiveMedia;
			case "FAVORITES":
			return TargetType.Favorites;
			case "LANGUAGE_SELECTOR":
			return TargetType.LanguageSelector;
			case "MENU":
			return TargetType.Menu;
			case "DOCUMENT":
			return TargetType.Document;
			case "WEBLINK":
			return TargetType.Weblink;
			case "CUSTOM":
			return TargetType.Custom;
			case "TICKETING":
			return TargetType.Ticketing;
			case "INDOOR_MAP":
			return TargetType.IndoorMap;
			case "RATE_APP":
			return TargetType.RateApp;
			case "RECOMMEND_APP":
			return TargetType.RecommendApp;
			case "OPEN_BOOK_FEED":
			return TargetType.OpenBookFeed;
			case "SOFTWARE_LICENSES":
			return TargetType.SoftwareLicenses;
			case "SEARCH":
			return TargetType.Search;
			case "UNITY":
			return TargetType.Unity;
			case "CALENDAR_VIEW":
			return TargetType.CalendarView;
			case "CALENDAR_EVENT":
			return TargetType.CalendarEvent;
			case "SETTINGS":
			return TargetType.Settings;
			}
			return null;
		}

		public override bool CanConvert(System.Type objectType) {
			return objectType == typeof(string);
		}
	}}
