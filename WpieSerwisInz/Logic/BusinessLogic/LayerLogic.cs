using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using WpieSerwisInz.Models;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models.BO.Core;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;
using WpieSerwisInz.Models.BO.Wrapper;

namespace WpieSerwisInz.Logic.BusinessLogic
{
    public static class LayerLogic
    {
        public static LayerTypeWrapper GetLayerWrapper(List<LayerView> layers, ApplicationUser user)
        {
            LayerTypeWrapper layer = new LayerTypeWrapper();
            layer.RadioButtons = new CustomRadioButtonList();
            layer.RadioButtons.Answers = GetLayers(layers);
            if (user.Layer != null)
            {
                layer.RadioButtons.SelectedValue = user.Layer.Id.ToString();
            }
            else
            {
                layer.RadioButtons.SelectedValue = "-1";
            }

            layer.UserId = user.Id;
            return layer;
        }

        private static List<Answer> GetLayers(List<LayerView> layers)
        {
            List<Answer> answers = new List<Answer>();
            answers.Add(new Answer() { Id = -1, Name = "Empty" });
            foreach (LayerView suvt in layers)
            {
                answers.Add(new Answer() { Id = suvt.Id, Name = suvt.LayerName });
            }
            return answers;
        }

        public static LayerViewWrapper GetLayerView()
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                LayerViewWrapper layer = new LayerViewWrapper();
                layer.SelectedElement = new List<LayerModel>();
                layer.AvailibleElement = GetListLayer(DbContext.LayerElementDbSet.ToList(), false);
                return layer;
            }
        }

        public static LayerViewWrapper GetLayerView(int layerId)
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                LayerViewWrapper layer = new LayerViewWrapper();
                var lay = DbContext.LayerViewDbSet.FirstOrDefault(x => x.Id == layerId);
                layer.SelectedElement = GetListLayer(lay.LayerElements.ToList(), true);
                layer.AvailibleElement = GetListLayer(DbContext.LayerElementDbSet.ToList(), false);

                layer.Name = lay.LayerName;
                layer.Information = lay.LayerInformation;

                return layer;
            }
        }

        public static LayerViewWrapper GetLayerViewMainPage(int layerId)
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                LayerViewWrapper layer = new LayerViewWrapper();
                var lay = DbContext.LayerViewDbSet.FirstOrDefault(x => x.Id == layerId);
                layer.SelectedElement = GetListLayer(lay.LayerElements.ToList(), true);
                layer.AvailibleElement = GetListLayer(lay.LayerElements.ToList(), false);

                layer.Name = lay.LayerName;
                layer.Information = lay.LayerInformation;

                return layer;
            }
        }

        public static List<LayerModel> GetListLayer(List<LayerElement> layersElem, bool selected)
        {
            List<LayerModel> layerModel = new List<LayerModel>();
            foreach (LayerElement elem in layersElem)
            {
                LayerModel model = GetLayerModel(elem);
                model.IsSelected = selected;
                layerModel.Add(model);
            }
            return layerModel;
        }

        public static LayerElement GetLayerElement(LayerModel lModel)
        {
            LayerElement model = new LayerElement()
            {
                Id = lModel.Id,
                ElementName = lModel.ElementName,
                Information = lModel.Information,
            };
            return model;
        }

        public static LayerModel GetLayerModel(LayerElement lElement)
        {
            LayerModel model = new LayerModel()
            {
                Id = lElement.Id,
                ElementName = lElement.ElementName,
                Information = lElement.Information,
            };
            return model;
        }

        public static LayerViewWrapper GetPostedService(LayerViewWrapper layer)
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                layer.AvailibleElement = GetListLayer(DbContext.LayerElementDbSet.ToList(), false);
                if (layer.PostedService.ServicesId.Any())
                {
                    var listElements = DbContext.LayerElementDbSet.ToList();
                    var selectedElement =
                        layer.PostedService.ServicesId.Select(
                            ids => listElements.FirstOrDefault(w => w.Id == Int32.Parse(ids)))
                            .Where(elem => elem != null)
                            .ToList();
                    layer.SelectedElement = GetListLayer(selectedElement, true);
                }
                return layer;
            }
        }

        public static List<LayerElement> GetPostedService(PostedService posted)
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                var listElements = DbContext.LayerElementDbSet.ToList();
                var selectedElement =
                    posted.ServicesId.Select(ids => listElements.FirstOrDefault(w => w.Id == Int32.Parse(ids)))
                        .Where(elem => elem != null)
                        .ToList();
                return selectedElement;
            }
        }

        public static MainPageWrapper GetPostedService(MainPageWrapper wrapper, int layerId)
        {
            using (ApplicationDbContext DbContext = new ApplicationDbContext())
            {
                var lay = DbContext.LayerViewDbSet.FirstOrDefault(x => x.Id == layerId);
                wrapper.AvailibleElement = GetListLayer(lay.LayerElements.ToList(), false);
                if (wrapper.PostedService.ServicesId.Any())
                {
                    var listElements = DbContext.LayerElementDbSet.ToList();
                    var selectedElement =
                        wrapper.PostedService.ServicesId.Select(
                            ids => listElements.FirstOrDefault(w => w.Id == Int32.Parse(ids)))
                            .Where(elem => elem != null)
                            .ToList();
                    wrapper.SelectedElement = GetListLayer(selectedElement, true);
                }
                return wrapper;
            }
        }
    }
}