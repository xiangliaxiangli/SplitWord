<template>
  <el-container>
    <el-main>
      <v-container>
        <!--      允许上传所有text文件格式-->
        <v-file-input
            label="请上传您想要分词检索的文件"
            accept="txt,pdf,doc,docx"
            v-model="file"
            outlined
            dense
        ></v-file-input>
        <el-card>
          <p class="green lighten-3 black--text text--darken-3">请选择您的文章的相关领域，如果其相关领域不存在于下列选项，则可以不做选择</p>
          <el-tag
              v-for="field in fields"
              :key="field.name"
              :type="field.type"
              effect="dark"
              @click="choose(field)">
            {{field.name}}
          </el-tag>
        </el-card>
        <el-main>
          <el-button @click="UpLoadFile" type="primary">上传<i class="el-icon-upload el-icon--right"></i></el-button>
        </el-main>
        <el-card class="tag-group">
          <p class="yellow lighten-3 black--text text--darken-3" :hidden=tips_hidden>以下是根据您上传的文章，为您检索出的关键词，您可以从中筛选，删除您不感兴趣的关键词</p>
          <el-tag
              :key="label"
              v-for="label in labels"
              closable
              :disable-transitions="false"
              @close="handleClose(label)">
            {{label}}
          </el-tag>
          <div></div>
        </el-card>
        <el-main>
          <el-button @click="UploadKeyWords" type="primary" icon="el-icon-search">提交</el-button>
        </el-main>
        <el-main>
          <el-button @click="test" type="primary" icon="el-icon-search">测试</el-button>
        </el-main>
      </v-container>
    </el-main>
  </el-container>
</template>

<script>
export default {
  name: "Main",
  data(){
    return{
      fields: [
        {name:"游戏",type:"info",ID:"0"},
        {name:"理工行业",type:"info",ID:"1"},
        {name:"人文社科",type:"info",ID:"2"},
        {name:"生活百科",type:"info",ID:"3"},
        {name:"体育运动",type:"info",ID:"4"},
        {name:"文化艺术",type:"info",ID:"5"},
        {name:"娱乐休闲",type:"info",ID:"6"}
      ],
      file: null,
      labels: [],
      tips_hidden: true
    }
  },
  methods:{
    test(){
      this.axios.post('https://localhost:44366/api/Hello/SplitText').then(response=>{
        console.log(response.data)
      })
    },
    //将用户上传的文件传输到后端
    UpLoadFile(){
      console.log(this.file.name)
      //创建FormData 对象
      let param = new FormData();
      let fields_;
      for(let i = 0;i<this.fields.length;i++){
        if(this.fields[i].type!=="info"){
          fields_ = this.fields[i].ID;
        }
      }
      param.append("file", this.file);
      param.append("fields",fields_);
      this.axios.post('https://localhost:44366/api/Hello/Post',param,{
        headers: {
          "Content-Type": "multipart/form-data"
        }
      }).then(response=>{
        console.log(response.data);
        console.log(response.data[0].word);
        for(let i = 0;i<response.data.length;i++){
          this.labels.push(response.data[i].word);
        }
        this.tips_hidden = false;
      })
    },
    //用户挑选完关键词之后，将剩下的词语传递会后端进行词语查找
    UploadKeyWords(){
      let param = new FormData();
      let keywords = JSON.stringify(this.labels);
      param.append("keywords",keywords);
      this.axios.post('https://localhost:44366/api/Hello/UploadKeyWords',param,{
        headers: {
          "Content-Type": "multipart/form-data"
        }
      }).then(response=>{
        console.log(response.data);
        if(response.data==="Success"){
          //上传成功
          this.$alert('上传知识成功，感谢您的慷慨！', '提示', {
            confirmButtonText: 'ok'
          })
        }else{
          this.$alert('系统出了点问题，程序员们正在抢救中', '提示', {
            confirmButtonText: 'ok'
          })
        }
      })
    },
    //关闭标签按钮
    handleClose(tag) {
      console.log(tag)
      this.labels.splice(this.labels.indexOf(tag), 1);
    },
    //选择感兴趣的领域
    choose(tag){
      console.log(tag)
      if(tag.type === "success"){
        tag.type = "info";
      }else{
        tag.type = "success";
      }
    }
  }
}
</script>

<style scoped>
.el-tag + .el-tag {
  margin-left: 10px;
}
.button-new-tag {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
}
.input-new-tag {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>