

CREATE  view [dbo].[VwModuleFeature]
AS
SELECT mf.[Id]
      ,mf.ModuleId
	  ,m.ModuleName
      ,mf.[ParentFeatureId]
	  ,mf.[FeatureName]
      ,mf.[NavURL]
      ,mf.[ShowInMenu]
      ,mf.[OrderId]
	  ,mf.IsActive
	  ,ISNULL((SELECT FeatureName FROM ModuleFeature modf 
			   WHERE modf.Id = mf.ParentFeatureId 
			   AND modf.ModuleId = mf.ModuleId),(SELECT ModuleName FROM Module module WHERE module.Id = mf.ModuleId)) AS ParentFeatureName
	   FROM ModuleFeature AS mf
	   INNER JOIN Module AS m ON mf.ModuleId = m.Id
