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
          <el-button @click="" type="primary" icon="el-icon-search">提交</el-button>
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
        {name:"计算机",type:"info"},
        {name:"数学",type:"info"},
        {name:"医学",type:"info"},
        {name:"财经",type:"info"},
        {name:"物理",type:"info"},
        {name:"化学",type:"info"}
      ],
      file: null,
      labels: null,
      tips_hidden: true
    }
  },
  methods:{
    //将用户上传的文件传输到后端
    UpLoadFile(){
      console.log(this.file.name)
      //创建FormData 对象
      let param = new FormData();
      let fields_ = [];
      for(let i = 0;i<this.fields.length;i++){
        if(this.fields[i].type!=="info"){
          fields_.push(this.fields[i].name);
        }
      }
      let temp = JSON.stringify(fields_);
      param.append("file", this.file);
      param.append("fields",temp);
      this.axios.post('https://localhost:44366/api/Hello/Post',param,{
        headers: {
          "Content-Type": "multipart/form-data"
        }
      }).then(response=>{
        console.log(response.data);
        this.labels = response.data;
        this.tips_hidden = false;
      })
    },
    //用户挑选完关键词之后，将剩下的词语传递会后端进行词语查找
    UploadKeyWords(){

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