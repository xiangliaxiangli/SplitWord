<template>
  <div class="login-container">
    <el-form :model="ruleForm2" :rules="rules2"
             status-icon
             ref="ruleForm2"
             label-position="left"
             label-width="0px"
             class="demo-ruleForm login-page">
      <h3 class="title">分词系统登录</h3>
      <el-form-item prop="username">
        <el-input type="text"
                  v-model="ruleForm2.username"
                  auto-complete="off"
                  placeholder="用户名"
        ></el-input>
      </el-form-item>
      <el-form-item prop="password">
        <el-input type="password"
                  v-model="ruleForm2.password"
                  auto-complete="off"
                  placeholder="密码"
        ></el-input>
      </el-form-item>
      <el-checkbox
          v-model="checked"
          class="rememberme"
      >记住密码</el-checkbox>
      <el-form-item style="width:100%;">
        <el-button type="primary" style="width:100%;" @click="handleSubmit" :loading="logining">登录</el-button>
      </el-form-item>
      <el-form-item style="width:100%;">
        <el-button type="primary" style="width:100%;" @click="logon" :loading="logining">注册</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
export default {
  data(){
    return {
      logining: false,
      ruleForm2: {
        username: null,
        password: null
      },
      rules2: {
        username: [{required: true, message: '请输入您的用户名', trigger: 'blur'}],
        password: [{required: true, message: '请输入您的密码', trigger: 'blur'}]
      },
      checked: false
    }
  },
  methods: {
    handleSubmit(event){
      this.$refs.ruleForm2.validate((valid) => {
        if(valid){
          this.logining = true;
          //用户名密码传给后台验证进行登录验证
          let username = this.ruleForm2.username;
          let password = this.ruleForm2.password;
          this.axios.post('https://localhost:44366/api/User/Login?userName='+username+'&password='+password).then(response=>{
            //打印返回的信息
            console.log("发起登录请求！");
            console.log(response.data);
            let result = response.data.split('|');
            console.log("result:"+result);
            console.log("result[0]:"+result[0]);
            console.log("result:[1]"+result[1]);

            if(result[0]==="success"){
              this.logining = false;
              //全局存储用户信息 后续有需要再考虑
              //sessionStorage.setItem('user', this.ruleForm2.username);
              //登录成功
              if(result[1]==="normal")
              {
                this.$router.push({path: '/Index'});
              }
              else if(result[1]==="admin")
              {
                //跳转到管理员界面
                console.log("跳转到管理员界面");
              }
              else{
                //错误
              }

            }else if(result[0]==="fail"){//当返回fail才表示用户存在，如果服务器没启动的话，也会是success之外的情况
              this.logining = false;
              this.$alert('用户名或者密码错误', '提示', {
                confirmButtonText: 'ok'
              })
            }
          })
        }else{
          console.log('系统出错啦！程序员正在努力维护中!');
          return false;
        }
      })
    },
    logon(){
      this.$router.push({path: '/Logon'});
    }
  }
};
</script>

<style scoped>
.login-container {
  width: 100%;
  height: 100%;
}
.login-page {
  -webkit-border-radius: 5px;
  border-radius: 5px;
  margin: 180px auto;
  width: 350px;
  padding: 35px 35px 15px;
  background:  #A0D0EC;
  border: 1px solid #eaeaea;
  box-shadow: 0 0 25px #cac6c6;
}
label.el-checkbox.rememberme {
  margin: 0px 0px 15px;
  text-align: left;
}
</style>