using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class StringSerializingInstaller : MonoInstaller
    {
        public override void InstallBindings() => InstallParserBindings(Container);

        protected virtual void InstallParserBindings(DiContainer subcontainer)
        {
            BindStringSplitter(subcontainer);
            BindMetadataListDeserializer(subcontainer);
            BindMetadataDeserializer(subcontainer);
            BindEncounterLockListDeserializer(subcontainer);
            BindEncounterLockDeserializer(subcontainer);
            BindSpriteDeserializer(subcontainer);

            BindBasicStatusDictionaryDeserializer(subcontainer);
            BindKeyedBasicStatusDeserializer(subcontainer);
            BindKeyedBasicStatusSerializer(subcontainer);


            BindNonImageContentDeserializer(subcontainer);
            BindImageContentDeserializer(subcontainer);
            BindXmlDocumentDeserializer(subcontainer);

            BindContentStatusDeserializer(subcontainer);
            BindSectionStatusDeserializer(subcontainer);
            BindTabStatusDeserializer(subcontainer);
            BindPanelStatusDeserializer(subcontainer);
            BindPinGroupStatusDeserializer(subcontainer);
            BindDialogueStatusDeserializer(subcontainer);
            BindQuizStatusDeserializer(subcontainer);
            BindKeyDeserializer(subcontainer);

            BindColorDeserializer(subcontainer);
            BindSpriteSerializer(subcontainer);
            BindMetadataSerializer(subcontainer);
        }

        protected virtual void BindStringSplitter(DiContainer subcontainer)
            => subcontainer.Bind<IStringSplitter>().To<DoubleColonStringSplitter>().AsTransient();
        protected virtual void BindMetadataListDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<List<EncounterMetadata>>>().To<ListDeserializer<EncounterMetadata>>().AsTransient();
        protected virtual void BindEncounterLockListDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<List<EncounterEditLock>>>().To<ListDeserializer<EncounterEditLock>> ().AsTransient();
        protected virtual void BindMetadataDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<EncounterMetadata>>().To<EncounterMetadataDeserializer>().AsTransient(); 
        protected virtual void BindEncounterLockDeserializer(DiContainer subcontainer)
             => subcontainer.Bind<IStringDeserializer<EncounterEditLock>>().To<EncounterEditLockDeserializer>().AsTransient();
        protected virtual void BindSpriteDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ISpriteDeserializer>().To<SpriteDeserializer>().AsTransient();
        protected virtual void BindBasicStatusDictionaryDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<Dictionary<int, EncounterBasicStatus>>>()
                           .To<DictionaryDeserializer<int, EncounterBasicStatus>>()
                           .AsTransient();
        protected virtual void BindKeyedBasicStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<KeyValuePair<int, EncounterBasicStatus>>>()
                           .To<KeyedEncounterStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindKeyedBasicStatusSerializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringSerializer<KeyValuePair<int, EncounterBasicStatus>>>()
                           .To<KeyedEncounterStatusSerializer>()
                           .AsTransient();

        protected virtual void BindNonImageContentDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<EncounterNonImageContent>>()
                           .To<XmlStringDeserializer<EncounterNonImageContent>>()
                           .AsTransient();
        protected virtual void BindImageContentDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<EncounterImageContent>>()
                           .To<XmlStringDeserializer<EncounterImageContent>>()
                           .AsTransient();
        protected virtual void BindXmlDocumentDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<XmlDocument>>().To<XmlDocumentDeserializer>().AsTransient();

        protected virtual void BindContentStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<EncounterContentStatus>>()
                           .To<EncounterContentStatusDeserializer>()
                           .AsTransient();


        protected virtual void BindSectionStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<SectionStatus>>()
                           .To<SectionStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindTabStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<TabStatus>>()
                           .To<TabStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindPanelStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<PanelStatus>>()
                           .To<PanelStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindPinGroupStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<PinGroupStatus>>()
                           .To<PinGroupStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindDialogueStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<DialogueStatus>>()
                           .To<DialogueStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindQuizStatusDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<QuizStatus>>()
                           .To<QuizStatusDeserializer>()
                           .AsTransient();
        protected virtual void BindKeyDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<ICharEnumeratorDeserializer<string>>().To<KeyDeserializer>().AsTransient();


        protected virtual void BindColorDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<Color>>().To<ColorDeserializer>().AsTransient();
        protected virtual void BindSpriteSerializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringSerializer<Sprite>>().To<SpriteSerializer>().AsTransient();
        protected virtual void BindMetadataSerializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringSerializer<EncounterMetadata>>().To<EncounterMetadataSerializer>().AsTransient();

        /// <summary>
        /// IL2CPP will strip generic constructors, so they have to be referenced in order to not be stripped.
        /// https://github.com/svermeulen/Extenject#aot-support
        /// </summary>
        public static void ForceIL2CPPToKeepNeededGenericConstructors()
        {
            new DictionaryDeserializer<int, EncounterBasicStatus>(null, null);
            new ListDeserializer<EncounterMetadata>(null, null);
            new ListDeserializer<EncounterEditLock>(null, null);
            new XmlStringDeserializer<EncounterNonImageContent>(null, null);
            new XmlStringDeserializer<EncounterImageContent>(null, null);
        }
    }
}