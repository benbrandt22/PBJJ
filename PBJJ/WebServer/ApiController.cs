﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using PBJJ.Core;

namespace PBJJ.WebServer
{
    [RestController(InstanceCreationType.PerCall)]
    public class ApiController
    {
        [UriFormat("/status")]
        public GetResponse GetApplicationStatus()
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, StatusProvider.GetCurrentStatus());
        }

        [UriFormat("/config")]
        public GetResponse GetApplicationConfiguration()
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, ConfigProvider.GetConfiguration());
        }

        [UriFormat("/saveConfig")]
        public IPostResponse SaveApplicationConfiguration([FromContent] PbjjConfigViewModel configViewModel)
        {
            ProgrammableBoxJointJigApp.Instance.KerfWidthInches = configViewModel.KerfWidthInches;
            ProgrammableBoxJointJigApp.Instance.MaxWidthInches = configViewModel.MaxWidthInches;

            return new PostResponse(PostResponse.ResponseStatus.Created,"index.html");
        }

        [UriFormat("/rehome")]
        public IPostResponse ReHome()
        {
            Task.Run(ProgrammableBoxJointJigApp.Instance.ReHome);
            return new PostResponse(PostResponse.ResponseStatus.Created, "");
        }

        [UriFormat("/toggleProfileMode")]
        public IPostResponse ToggleProfileMode()
        {
            ProgrammableBoxJointJigApp.Instance.ToggleProfileMode();
            return new PostResponse(PostResponse.ResponseStatus.Created, "");
        }

        [UriFormat("/runProgram")]
        public IPostResponse RunProgram()
        {
            Task.Run(ProgrammableBoxJointJigApp.Instance.RunProgram);
            return new PostResponse(PostResponse.ResponseStatus.Created, "");
        }

        [UriFormat("/profiles")]
        public GetResponse GetProfiles()
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, ProfileManager.GetProfiles());
        }

        [UriFormat("/createNew")]
        public IPostResponse CreateNewProfile([FromContent] NewProfileViewModel newProfileViewModel) {
            ProfileManager.CreateNewProfile(newProfileViewModel);

            return new PostResponse(PostResponse.ResponseStatus.Created, "profiles.html");
        }

        [UriFormat("/loadProfile")]
        public IPostResponse LoadProfile([FromContent] LoadProfileViewModel loadProfileViewModel)
        {
            ProfileManager.LoadProfileByName(loadProfileViewModel.Name);

            return new PostResponse(PostResponse.ResponseStatus.Created, "profiles.html");
        }

        [UriFormat("/getProfileData/{fileName}")]
        public GetResponse GetProfileData(string fileName) {
            fileName = Uri.UnescapeDataString(fileName);
            return new GetResponse(GetResponse.ResponseStatus.OK, ProfileManager.GetProfileData(fileName));
        }

        [UriFormat("/save")]
        public async Task<IPostResponse> SaveProfile([FromContent] SaveProfileViewModel saveProfileViewModel) {
            await ProfileManager.SaveFile(saveProfileViewModel.Name, saveProfileViewModel.Data);
            return new PostResponse(PostResponse.ResponseStatus.Created, "profiles.html");
        }

        [UriFormat("/rename")]
        public async Task<IPostResponse> RenameProfile([FromContent] RenameViewModel renameViewModel)
        {
            await ProfileManager.RenameFile(renameViewModel.OldName, renameViewModel.NewName);
            return new PostResponse(PostResponse.ResponseStatus.Created, "profiles.html");
        }

        [UriFormat("/delete")]
        public async Task<IPostResponse> DeleteProfile([FromContent] DeleteViewModel deleteViewModel)
        {
            await ProfileManager.DeleteFile(deleteViewModel.Name);
            return new PostResponse(PostResponse.ResponseStatus.Created, "profiles.html");
        }

    }
}
