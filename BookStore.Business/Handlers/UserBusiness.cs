﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Business.DataAccess;
using BookStore.Business.Interface;
using BookStore.Business.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Business.Handlers
{
    public class UserBusiness : IUserBusiness
    {
        private readonly ISqlDataAccessHelper sqlDataAccessHelper;
        public UserBusiness(ISqlDataAccessHelper sqlDataAccessHelper)
        {
            this.sqlDataAccessHelper = sqlDataAccessHelper;
        }
        public async Task<int> ValidateUser(LoginViewModel loginModel)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "dbo.ValidateUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", loginModel.Email);
                cmd.Parameters.AddWithValue("@Password", loginModel.Password);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                await this.sqlDataAccessHelper.ExecuteNonQueryAsync(cmd);
                int result = Convert.ToInt32(cmd.Parameters["@ID"].Value);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<bool> UserRegistration(UserModel userModel)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = "RegisterUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", userModel.Username);
            cmd.Parameters.AddWithValue("@Email", userModel.Email);
            cmd.Parameters.AddWithValue("@Address", userModel.Address);
            cmd.Parameters.AddWithValue("@Phone", userModel.Phone);
            cmd.Parameters.AddWithValue("@Password", userModel.Password);
            var status = await this.sqlDataAccessHelper.ExecuteNonQueryAsync(cmd) > 0;
            return status;
        }

    }
}
