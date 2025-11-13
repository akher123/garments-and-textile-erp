using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class ColorManager : IColorManager
    {
        private readonly IColorRepository _colorRepository;
      
        public ColorManager(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public int SaveColor(Color model)
        {
           return _colorRepository.Save(model);
        }

        public List<Color> GetColorstByPaging(Color model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var colorList =
                _colorRepository.Filter(
                    x =>
                        (x.ColorRef.Trim().ToLower().Contains(model.ColorName.Trim().ToLower()) || String.IsNullOrEmpty(model.ColorName.Trim())) || (x.ColorCode.Trim().Contains(model.ColorName.Trim()) || String.IsNullOrEmpty(model.ColorName.Trim())) ||
                         (x.ColorName.Trim().Contains(model.ColorName.Trim())) || String.IsNullOrEmpty(model.ColorName.Trim()));
                                             
            totalRecords= colorList.Count();

            if (totalRecords > 0)
            {
                switch (model.sort)
                {
                    case "ColorName":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                colorList = colorList
                                    .OrderByDescending(r => r.ColorName)
                                    .Skip(index*pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                colorList = colorList
                                    .OrderBy(r => r.ColorName)
                                    .Skip(index*pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "ColorRef":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                colorList = colorList
                                    .OrderByDescending(r => r.ColorRef)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                colorList = colorList
                                    .OrderBy(r => r.ColorRef)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    case "ColorCode":
                        switch (model.sortdir)
                        {
                            case "DESC":
                                colorList = colorList
                                    .OrderByDescending(r => r.ColorCode)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);

                                break;
                            default:
                                colorList = colorList
                                    .OrderBy(r => r.ColorCode)
                                    .Skip(index * pageSize)
                                    .Take(pageSize);
                                break;
                        }
                        break;
                    default:
                        colorList = colorList
                            .OrderByDescending(r => r.ColorId)
                            .Skip(index*pageSize)
                            .Take(pageSize);
                        break;
                }
            }
            return colorList.ToList();
        }

        public Color GetColorById(long colorId)
        {
            return _colorRepository.FindOne(x => x.ColorId == colorId);
        }

        public int EditColor(Color model)
        {
            var color = _colorRepository.FindOne(x => x.ColorId == model.ColorId);
            color.ColorCode = model.ColorCode;
            color.ColorName = model.ColorName;
            color.ColorRef = model.ColorRef;
           return _colorRepository.Edit(color);
        }

        public string GetNewColorRef()
        {
            var colorRef = _colorRepository.All().Max(x => x.ColorRef);
            return Convert.ToString((Convert.ToInt32(colorRef) + 1));
        }

        public List<Color> GetColors()
        {
            return _colorRepository.All().OrderBy(x=>x.ColorName).ToList();
        }

        public object AutoCompliteColor(string searchString)
        {
            return
                _colorRepository.Filter(
                    x => x.ColorName.Trim().Replace(" ", String.Empty).StartsWith(searchString.Replace(" ", String.Empty))).Take(10).OrderBy(x=>x.ColorName);
        }

        public int DeleteColor(long colorId)
        {
            return _colorRepository.Delete(x => x.ColorId == colorId);
        }
    }
}
