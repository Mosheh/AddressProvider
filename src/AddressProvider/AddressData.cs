﻿using AddressProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressProvider.Extensions;
using AddressProvider.Models.TargetLock;

namespace AddressProvider
{
    /// <summary>
    /// Data model with information about address requested
    /// </summary>
    public class AddressData
    {
        public AddressData()
        {
            StateInf = new StateInformation();
            CityInfo = new CityInformation();
        }

        internal void FillBy(PostmonModel model)
        {
            if (model == null)
                throw new Exception("Modelo inválido");
            if (CityInfo == null)
                throw new Exception("Modelo do cidade inválido");
            if (StateInf == null)
                throw new Exception("Modelo do estado inválido");
            this.Complementary = model.complemento;
            this.Neighborhood = model.bairro==null? "":model.bairro;
            this.City = model.cidade == null ? "" : model.cidade; ;
            this.Street = model.logradouro == null ? "": model.logradouro;
            this.ZipCode = model.cep == null ? "" : model.cep; ;
            this.State = model.estado==null ? "": model.estado;
            if (model.estado_info == null)
                model.estado_info = new Estado_info();

            SetStateInf(model.estado_info.area_km2.To<decimal>(), model.estado_info.codigo_ibge, model.estado_info.nome);
            if (model.cidade_info == null)
                model.cidade_info = new Cidade_info();
            SetCityInformation(model.cidade_info.area_km2.To<decimal>(), model.cidade_info.codigo_ibge);
        }

        /// <summary>
        /// Complemento
        /// </summary>
        public string Complementary { get; set; }

        /// <summary>
        /// Bairro
        /// </summary>
        public string Neighborhood { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string State { get; set; }

        public StateInformation StateInf { get; private set; }
        public CityInformation CityInfo { get; private set; }

        public void SetStateInf(decimal areaKm2, string ibgeCode, string name)
        {
            StateInf.AreaKm2 = areaKm2;
            StateInf.IBGECode = ibgeCode;
            StateInf.Name = name;
        }

        public void SetCityInformation(decimal areaKm2, string ibgeCode)
        {
            this.CityInfo.AreaKm2 = areaKm2;
            this.CityInfo.IBGECode = ibgeCode;
        }

        internal void FillBy(Models.ViaCEP.ViaCEPModel viaCEPModel)
        {
            if (viaCEPModel == null)
                throw new Exception("Modelo inválido");

            this.ZipCode = viaCEPModel.cep;
            this.State = viaCEPModel.uf;
            this.City = viaCEPModel.localidade;
            this.Neighborhood = viaCEPModel.bairro;
            this.Complementary = viaCEPModel.complemento;
            this.CityInfo.IBGECode = viaCEPModel.ibge;
            this.Street = viaCEPModel.logradouro;
        }

        internal void FillBy(TargetLockAddressModel targetModel)
        {
            if (targetModel == null)
                throw new Exception("Modelo inválido");

            this.ZipCode = targetModel.post_code;
            this.State =  targetModel.admin_level_1_short;
            this.City = targetModel.locality;
            this.Neighborhood = targetModel.admin_level_2;
            this.Complementary = targetModel.post_code_type;
            this.CityInfo.IBGECode = "";
            this.StateInf.Name = targetModel.admin_level_1_long;
        }
    }
}
