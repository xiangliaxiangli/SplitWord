<template>
  <el-main class="login-container">
    <el-form :model="ruleForm" status-icon :rules="rules" ref="ruleForm" label-width="100px" class="demo-ruleForm login-page">
      <el-form-item label="用户名" prop="username">
        <el-input type="username" v-model="ruleForm.username" autocomplete="off"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="pass">
        <el-input type="password" v-model="ruleForm.pass" autocomplete="off"></el-input>
      </el-form-item>
      <el-form-item label="确认密码" prop="checkPass">
        <el-input type="password" v-model="ruleForm.checkPass" autocomplete="off"></el-input>
      </el-form-item>
      <el-form-item label="性别" prop="sex">
        <el-radio-group v-model="ruleForm.sex">
          <el-radio label="男"></el-radio>
          <el-radio label="女"></el-radio>
        </el-radio-group>
      </el-form-item>
      <!--      <el-form-item label="性别" prop="sex">-->
      <!--        <el-input v-model.number="ruleForm.sex"></el-input>-->
      <!--      </el-form-item>-->
      <el-form-item>
        <el-button type="primary" style="background-color: #A0D0EC" @click="submitForm('ruleForm')">提交</el-button>
        <el-button  @click="resetForm('ruleForm')">重置</el-button>
      </el-form-item>
    </el-form>
  </el-main>
</template>

<script>
export default {
  name: 'Logon',
  data() {
    let validateUsername = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('姓名不能为空'));
      }
      setTimeout(() => {
        callback();
      }, 1000);
    };
    let checkAge = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('年龄不能为空'));
      }
      setTimeout(() => {
        callback();
      }, 1000);
    };
    let validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入密码'));
      } else {
        if (this.ruleForm.checkPass !== '') {
          this.$refs.ruleForm.validateField('checkPass');
        }
        callback();
      }
    };
    let validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请再次输入密码'));
      } else if (value !== this.ruleForm.pass) {
        callback(new Error('两次输入密码不一致!'));
      } else {
        callback();
      }
    };
    return {
      ruleForm: {
        username: '',
        pass: '',
        checkPass: '',
        sex: ''
      },
      rules: {
        username: [
          { validator: validateUsername,trigger: 'blur'}
        ],
        pass: [
          { validator: validatePass, trigger: 'blur' }
        ],
        checkPass: [
          { validator: validatePass2, trigger: 'blur' }
        ],
        sex: [
          { validator: checkAge, trigger: 'blur' }
        ]
      }
    };
  },
  methods: {
    //用户注册
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if(valid===true){
          let username = this.ruleForm.username;
          let password = this.ruleForm.pass;
          let sex = this.ruleForm.sex;
          alert("即将向后台进行验证")
          //返回注册的内容到前端
          this.axios.post('https://localhost:44366/api/User/Register?userName='+username+'&password='+password+'&sex='+sex).then(response=>{
            console.log("发起注册请求");

            console.log("response:"+response.data);
            let result = response.data;
            if(result==="success"){
              this.logining = false;
              //全局存储用户信息 后续有需要再考虑
              //sessionStorage.setItem('user', this.ruleForm2.username);
              //登录成功
              this.$alert('注册成功！', '注册', {
                confirmButtonText: 'ok'
              })
              this.$router.push({path: '/'});
            }else if(result==="fail"){
              this.logining = false;
              this.$alert('该用户名已经存在', '注册', {
                confirmButtonText: 'ok'
              })
            }
          })
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    }
  }
}
</script>

<style scoped>
.login-container {
  width: 100%;
  height: 100%;
  background-color: #6E9BCC;
}
.login-page {
  -webkit-border-radius: 5px;
  border-radius: 5px;
  margin: 180px auto;
  width: 550px;
  height: 350px;
  padding: 35px 35px 15px;
  background:  #A0D0EC;
  border: 1px solid #eaeaea;
//box-shadow: 0 0 25px #cac6c6;
}
</style>