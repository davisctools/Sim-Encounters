﻿using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class XmlSerializationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindImageContentSerializer(Container);
            BindNonImageSerializer(Container);
        }

        protected virtual void BindImageContentSerializer(DiContainer subcontainer)
        {
            subcontainer.Bind<IXmlSerializer<EncounterImageContent>>().To<ImageContentXmlSerializer>().AsTransient();
            BindSpriteSerializer(subcontainer);
        }
        protected virtual void BindSpriteSerializer(DiContainer subcontainer)
            => subcontainer.Bind<IXmlSerializer<Sprite>>().To<SpriteXmlSerializer>().AsTransient();

        protected virtual void BindNonImageSerializer(DiContainer subcontainer)
        {
            subcontainer.Bind<IXmlSerializer<EncounterNonImageContent>>().To<NonImageContentXmlSerializer>().AsTransient();
            BindSectionSerializer(subcontainer);
        }
        protected virtual void BindSectionSerializer(DiContainer subcontainer)
        {
            subcontainer.Bind<IXmlSerializer<Section>>().To<SectionXmlSerializer>().AsTransient();
            BindTabSerializer(subcontainer);
        }
        protected virtual void BindTabSerializer(DiContainer subcontainer)
        {
            subcontainer.Bind<IXmlSerializer<Tab>>().To<TabXmlSerializer>().AsTransient();
            BindPanelSerializer(subcontainer);
        }
        protected virtual void BindPanelSerializer(DiContainer subcontainer)
        {
            subcontainer.Bind<IXmlSerializer<Panel>>().To<PanelXmlSerializer>().AsTransient();
            BindPinGroupSerializer(subcontainer);
        }
        protected virtual void BindPinGroupSerializer(DiContainer subcontainer)
        {
            subcontainer.Bind<IXmlSerializer<PinGroup>>().To<PinGroupXmlSerializer>().AsTransient();
            BindDialoguePinSerializer(subcontainer);
            BindQuizPinSerializer(subcontainer);
        }
        protected virtual void BindDialoguePinSerializer(DiContainer subcontainer)
            => subcontainer.Bind<IXmlSerializer<DialoguePin>>().To<DialoguePinXmlSerializer>().AsTransient();
        protected virtual void BindQuizPinSerializer(DiContainer subcontainer)
            => subcontainer.Bind<IXmlSerializer<QuizPin>>().To<QuizPinXmlSerializer>().AsTransient();
    }
}